using Naukri;
using Naukri.Unity.AwaitCoroutine;
using Naukri.Unity.BetterAttribute;
using Naukri.Unity.Singleton;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Naukri.Unity.SceneManagement
{
    public delegate void LoadingSceneEvent(Scene scene);

    public class SceneManager : SingletonBehaviour<SceneManager>
    {
        private enum TargetState
        {
            Load,
            UnLoad
        }

        private static readonly Queue<(Scene, TargetState)> _processQueue = new Queue<(Scene, TargetState)>();

        public static event LoadingSceneEvent OnLoadingSceneStart;

        public static event LoadingSceneEvent OnLoadingSceneEnd;

        public static event LoadingSceneEvent OnUnLoadingSceneStart;

        public static event LoadingSceneEvent OnUnLoadingSceneEnd;

        [ReadOnly, SerializeField]
        public bool isBusy = false;

        public static bool IsBusy { get => Instance.isBusy; set => Instance.isBusy = value; }


        [SerializeField]
        private SceneObject[] firstLoadScenes;

        public void Awake()
        {
            LoadScene(firstLoadScenes);
        }

        private static void AddProcess(Scene scene, TargetState targetState)
        {
            _processQueue.Enqueue((scene, targetState));
            TryHandleProcess();
        }

        public static void TryHandleProcess()
        {
            static async void HandleProcess()
            {
                IsBusy = true;
                while (_processQueue.Count > 0) // 執行直到佇列中沒有處理對象
                {
                    (var scene, var targetState) = _processQueue.Dequeue();

                    switch (targetState)
                    {
                        case TargetState.Load:
                            await LoadSceneAsyncImp(scene);
                            break;
                        case TargetState.UnLoad:
                            await UnLoadSceneAsyncImp(scene);
                            break;
                        default:
                            break;
                    }
                }
                IsBusy = false;
            }

            if (!IsBusy) // 只在閒置時觸發處理器
            {
                HandleProcess();
            }
        }

        private static async Task LoadSceneAsyncImp(Scene scene)
        {
            if (!scene.isLoaded)
            {
                OnLoadingSceneStart(scene);
                await USceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Additive);
                OnLoadingSceneEnd(scene);
            }
        }

        private static async Task UnLoadSceneAsyncImp(Scene scene)
        {
            if (scene.isLoaded)
            {
                OnUnLoadingSceneStart(scene);
                await USceneManager.UnloadSceneAsync(scene.buildIndex);
                OnUnLoadingSceneEnd(scene);
            }
        }

        public static void LoadScene(params SceneObject[] sceneObjects)
        {
            foreach (var sceneObject in sceneObjects)
            {
                AddProcess(sceneObject, TargetState.Load);
            }
        }

        public static void UnLoadScene(params SceneObject[] sceneObjects)
        {
            foreach (var sceneObject in sceneObjects)
            {
                AddProcess(sceneObject, TargetState.UnLoad);
            }
        }
    }
}