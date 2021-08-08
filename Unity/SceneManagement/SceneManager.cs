using Naukri;
using Naukri.Extensions;
using Naukri.Unity.AwaitCoroutine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Naukri.Unity.SceneManagement
{
    public delegate void LoadingSceneEvent(string sceneName);

    public class SceneManager
    {
        private enum ProcessType
        {
            LoadScene,
            LoadAndDisable,
            UnloadScene
        }

        private static readonly Queue<(string, ProcessType)> processQueue = new Queue<(string, ProcessType)>();

        private static readonly Dictionary<string, List<bool>> activeSnapshots = new Dictionary<string, List<bool>>();

        public static bool IsBusy { get; set; }

        public static event LoadingSceneEvent OnLoadingSceneStart;

        public static event LoadingSceneEvent OnLoadingSceneEnd;

        public static event LoadingSceneEvent OnUnLoadingSceneStart;

        public static event LoadingSceneEvent OnUnLoadingSceneEnd;

        private static void AddProcess(string sceneName, ProcessType targetState)
        {
            processQueue.Enqueue((sceneName, targetState));
            TryHandleProcess();
        }

        public static void TryHandleProcess()
        {
            static async void HandleProcess()
            {
                IsBusy = true;
                while (processQueue.Count > 0) // 執行直到佇列中沒有處理對象
                {
                    (var sceneName, var targetState) = processQueue.Dequeue();

                    switch (targetState)
                    {
                        case ProcessType.LoadScene:
                        case ProcessType.LoadAndDisable:
                            await LoadSceneAsyncImp(sceneName);
                            if (targetState == ProcessType.LoadAndDisable)
                            {
                                DisableScene(sceneName);
                            }
                            break;
                        case ProcessType.UnloadScene:
                            await UnloadSceneAsyncImp(sceneName);
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

        private static async Task LoadSceneAsyncImp(string sceneName)
        {
            if (!USceneManager.GetSceneByName(sceneName).isLoaded) // Unloaded (index 為 -1) 的場景 isLoaded 為 False 剛好可以通過
            {
                OnLoadingSceneStart?.Invoke(sceneName);
                await USceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                OnLoadingSceneEnd?.Invoke(sceneName);
            }
        }

        private static async Task UnloadSceneAsyncImp(string sceneName)
        {
            if (USceneManager.GetSceneByName(sceneName).isLoaded)
            {
                OnUnLoadingSceneStart?.Invoke(sceneName);
                await USceneManager.UnloadSceneAsync(sceneName);
                OnUnLoadingSceneEnd?.Invoke(sceneName);
            }
        }

        public static void LoadScene(string sceneName)
        {
            AddProcess(sceneName, ProcessType.LoadScene);
        }

        public static void UnloadScene(string sceneName)
        {
            AddProcess(sceneName, ProcessType.UnloadScene);
        }

        public static void EnableScene(string sceneName)
        {
            var scene = USceneManager.GetSceneByName(sceneName);
            if (scene.IsValid())
            {
                if (activeSnapshots.TryGetValue(sceneName, out var snapshots))
                {
                    scene.GetRootGameObjects()
                        .ForEach((it, i) => it.SetActive(snapshots[i]));
                    activeSnapshots.Remove(sceneName);
                }
            }
        }

        public static void DisableScene(string sceneName)
        {
            var scene = USceneManager.GetSceneByName(sceneName);
            if (scene.IsValid())
            {
                var snapshots = new List<bool>();
                scene.GetRootGameObjects()
                    .CreateSnapshots(it => it.activeSelf, snapshots)
                    .ForEach(it => it.SetActive(false));
                activeSnapshots.Add(sceneName, snapshots);
            }
        }

        public static void EnableOrLoadScene(string sceneName)
        {
            var scene = USceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded)
            {
                EnableScene(sceneName);
            }
            else
            {
                LoadScene(sceneName);
            }
        }

        public static void LoadAndDisableScene(string sceneName)
        {
            AddProcess(sceneName, ProcessType.LoadAndDisable);
        }

        public static void HandleByLoadingMode(string sceneName, LoadingMode loadingMode)
        {
            switch (loadingMode)
            {
                case LoadingMode.None:
                    break;
                case LoadingMode.Load:
                    LoadScene(sceneName);
                    break;
                case LoadingMode.Unload:
                    UnloadScene(sceneName);
                    break;
                case LoadingMode.Enable:
                    EnableScene(sceneName);
                    break;
                case LoadingMode.Disable:
                    DisableScene(sceneName);
                    break;
                case LoadingMode.EnableOrLoad:
                    EnableOrLoadScene(sceneName);
                    break;
                case LoadingMode.LoadAndDisable:
                    LoadAndDisableScene(sceneName);
                    break;
                default:
                    break;
            }
        }
    }
}