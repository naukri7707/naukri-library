using Naukri.BetterAttribute;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Naukri.SceneManagement
{
    public class AdditiveSceneLoader2D : MonoBehaviour
    {
        private enum LoadType { Simple, Normal, Advance }

        private enum Scope { Unload, Buffer, Load }

        [SerializeField]
        private LoadType loadType;

        private Scope scope;

        [SerializeField, ExpandElement]
        private SceneObject scene;

        [SerializeField, DisplayWhenFieldEqual(nameof(loadType), LoadType.Normal, LoadType.Advance)]
        private Transform target;

        [SerializeField, DisplayWhenFieldEqual(nameof(loadType), LoadType.Normal, LoadType.Advance)]
        private Collider2D loadScope;

        [SerializeField, DisplayWhenFieldEqual(nameof(loadType), LoadType.Advance)]
        private float bufferWidth;

        private void OnValidate()
        {
            if(loadScope != null)
            {
                loadScope.isTrigger = true;
            }
            if(bufferWidth < 0F)
            {
                bufferWidth = 0F;
            }
        }

        public IEnumerator Start()
        {
            if(loadType != LoadType.Simple && (target == null || loadScope == null))
            {
                yield break;
            }
            for (; ; )
            {
                CheckScope();
                yield return LoadScene();
                yield return null;
            }
        }

        private IEnumerator LoadScene()
        {
            switch (scope)
            {
                case Scope.Unload:
                    if (SceneManager.GetSceneByName(scene).isLoaded)
                    {
                        yield return SceneManager.UnloadSceneAsync(scene);
                    }
                    break;
                case Scope.Load:
                    if (!SceneManager.GetSceneByName(scene).IsValid())
                    {
                        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
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
                case LoadType.Simple:
                    scope = Scope.Load;
                    break;
                case LoadType.Normal:
                case LoadType.Advance:
                    var targetPos = target.position;
                    var closestPoint = loadScope.ClosestPoint(targetPos);
                    var distance = Vector2.Distance(closestPoint, targetPos);
                    Debug.Log($"C={closestPoint}, D={distance}");
                    if (distance == 0)
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