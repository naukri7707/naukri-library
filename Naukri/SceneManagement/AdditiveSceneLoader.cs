using Naukri;
using Naukri.BetterAttribute;
using Naukri.BetterInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Naukri.SceneManagement
{
    public class AdditiveSceneLoader : MonoBehaviour
    {
        private enum LoadType { Simple, Normal, Advance }

        private enum Scope { Unload, Buffer, Load }

        private const float DEFAULT_BUFFER_WIDTH = 0F;

        [SerializeField]
        private LoadType loadType;

        private Scope scope;

        [SerializeField, ExpandElement]
        private SceneObject scene;

        [SerializeField, DisplayWhenFieldEqual(nameof(loadType), LoadType.Normal, LoadType.Advance)]
        private Transform target;

        [SerializeField, DisplayWhenFieldEqual(nameof(loadType), LoadType.Normal, LoadType.Advance)]
        private Collider loadScope;

        [SerializeField, DisplayWhenFieldEqual(nameof(loadType), LoadType.Advance)]
        private float bufferWidth = DEFAULT_BUFFER_WIDTH;

        public IEnumerator Start()
        {
            InitLoader();
            for (; ; )
            {
                CheckScope();
                yield return LoadScene();
                yield return null;
            }
        }

        private void InitLoader()
        {
            switch (loadType)
            {
                case LoadType.Simple:
                    scope = Scope.Load;
                    break;
                case LoadType.Normal:
                    bufferWidth = DEFAULT_BUFFER_WIDTH;
                    break;
                case LoadType.Advance:
                    if (bufferWidth < 0F)
                    {
                        bufferWidth = DEFAULT_BUFFER_WIDTH;
                    }
                    break;
                default:
                    break;
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
                case LoadType.Normal:
                case LoadType.Advance:
                    var targetPos = target.position;
                    var closestPoint = loadScope.ClosestPoint(targetPos);
                    var distance = Vector2.Distance(closestPoint, targetPos);
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