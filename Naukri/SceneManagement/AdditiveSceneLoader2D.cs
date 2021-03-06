using Naukri.BetterAttribute;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Naukri.SceneManagement
{
    public class AdditiveSceneLoader2D : MonoBehaviour
    {
        private enum LoadType { Unload, Load, Zone, ZoneWithBuffer }

        private enum Scope { Unload, Buffer, Load }

        [SerializeField]
        private LoadType loadType;

        private Scope scope;

        [SerializeField, ExpandElement]
        private SceneObject scene;

        [SerializeField, DisplayWhenFieldEqual(nameof(loadType), LoadType.Zone, LoadType.ZoneWithBuffer)]
        private Transform target;

        [SerializeField, DisplayWhenFieldEqual(nameof(loadType), LoadType.Zone, LoadType.ZoneWithBuffer)]
        private Collider2D loadZone;

        [SerializeField, DisplayWhenFieldEqual(nameof(loadType), LoadType.ZoneWithBuffer)]
        private float bufferWidth;

        private void OnValidate()
        {
            if (loadZone != null)
            {
                loadZone.isTrigger = true;
                loadZone.enabled = false;
            }
            if (bufferWidth < 0F)
            {
                bufferWidth = 0F;
            }
        }

        private void Update()
        {
            CheckScope();
            LoadScene();
        }

        private void LoadScene()
        {
            switch (scope)
            {
                case Scope.Unload:
                    if (SceneManager.GetSceneByName(scene).isLoaded)
                    {
                        SceneManager.UnloadSceneAsync(scene);
                    }
                    break;
                case Scope.Load:
                    if (!SceneManager.GetSceneByName(scene).IsValid())
                    {
                        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                    }
                    break;
                default:
                    break;
            }
        }

        private void CheckScope()
        {
            switch (loadType)
            {
                case LoadType.Unload:
                    scope = Scope.Unload;
                    break;
                case LoadType.Load:
                    scope = Scope.Load;
                    break;
                case LoadType.Zone:
                case LoadType.ZoneWithBuffer:
                    if (target == null || loadZone == null)
                    {
                        throw new UnityException($"{nameof(AdditiveSceneLoader2D)}'s {nameof(target)} and {nameof(loadZone)} can not be null in {loadType} mode.");
                    }
                    var targetPos = target.position;
                    var closestPoint = loadZone.ClosestPoint(targetPos);
                    var distance = Vector2.Distance(closestPoint, targetPos);
                    if (distance is 0F)
                    {
                        scope = Scope.Load;
                    }
                    else if (distance < bufferWidth)
                    {
                        scope = Scope.Buffer;
                    }
                    else
                    {
                        scope = Scope.Unload;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}