using Naukri;
using Naukri.AwaitCoroutine;
using Naukri.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Naukri.SceneManagement
{
    public delegate void SceneChangeCallback(Scene scene);

    public static class SceneManager
    {
        private const string DISABLE_ROOT_NAME = "[Disabled]";

        private static readonly Queue<(Scene, TargetState)> processQueue = new Queue<(Scene, TargetState)>();

        public static Queue<(Scene, TargetState)> ProcessQueue => processQueue;

        private static readonly KeyList<string, Scene> buildSettingsScenes;

        public static KeyList<string, Scene> BuildSettingsScenes => buildSettingsScenes;

        private static bool isBusy;

        public static bool IsBusy;

        private static float progress;

        public static float Progress => progress;

        public static event SceneChangeCallback OnSceneUnloaded = s => s.loadingState = LoadingState.Unloaded;

        public static event SceneChangeCallback OnSceneLoaded = s => s.loadingState = LoadingState.Loaded;

        public static event SceneChangeCallback OnSceneDisabled = s => s.loadingState = LoadingState.Disabled;

        public static event SceneChangeCallback OnSceneLoading = s => s.loadingState = LoadingState.Loading;

        public static event SceneChangeCallback OnSceneUnloading = s => s.loadingState = LoadingState.Unloading;

        static SceneManager()
        {
            buildSettingsScenes = GetAllBuildSettingsScenes();
            foreach (var scene in buildSettingsScenes)
            {
                if (USceneManager.GetSceneByBuildIndex(scene.buildIndex).isLoaded)
                {
                    OnSceneLoaded(scene);
                }
            }
        }

        private static readonly Func<Scene, Task>[] processMethod = new Func<Scene, Task>[3]
        {
            // Unloaded
            s => UnloadSceneAsync(s),
            // Loaded
            s => LoadSceneAsync(s),
            // Disabled
            s => DisableSceneAsync(s),
        };

        public static void LoadScene(int buildIndex) => LoadScene(GetSceneByBuildIndex(buildIndex));

        public static void LoadScene(string sceneName) => LoadScene(GetSceneByName(sceneName));

        public static void LoadScene(Scene scene) => HandleScene(scene, TargetState.Load);

        private static async Task LoadSceneAsync(Scene scene)
        {
            switch (scene.loadingState)
            {
                case LoadingState.Unloaded:
                case LoadingState.Unloading:
                    if (scene.loadingState is LoadingState.Unloading)
                    {
                        await new WaitUntil(() => scene.loadingState is LoadingState.Unloaded);
                    }
                    OnSceneLoading(scene);
                    var asop = USceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Additive);
                    while (!asop.isDone)
                    {
                        progress = asop.progress;
                        await new WaitForUpdate();
                    }
                    OnSceneLoaded(scene);
                    break;
                case LoadingState.Loading:
                case LoadingState.Loaded:
                    // Do nothing
                    break;
                case LoadingState.Disabled:
                    var uscene = USceneManager.GetSceneAt(scene.buildIndex);
                    var disabled = uscene.GetRootGameObjects()
                        .FirstOrDefault(it => it.name is DISABLE_ROOT_NAME);
                    if (disabled)
                    {
                        foreach (Transform child in disabled.transform)
                        {
                            child.parent = null;
                        }
                        GameObject.Destroy(disabled);
                    }
                    OnSceneLoaded(scene);
                    break;
                default:
                    break;
            }
        }

        public static void DisableScene(int buildIndex) => DisableScene(GetSceneByBuildIndex(buildIndex));

        public static void DisableScene(string sceneName) => DisableScene(GetSceneByName(sceneName));

        public static void DisableScene(Scene scene) => HandleScene(scene, TargetState.Disable);

        private static async Task DisableSceneAsync(Scene scene)
        {
            if (scene.loadingState is LoadingState.Disabled) return;
            await LoadSceneAsync(scene);
            var uscene = USceneManager.GetSceneAt(scene.buildIndex);
            var disabled = new GameObject(DISABLE_ROOT_NAME);
            disabled.SetActive(false);
            USceneManager.MoveGameObjectToScene(disabled, uscene);
            foreach (GameObject go in uscene.GetRootGameObjects())
            {
                go.transform.SetParent(disabled.transform, false);
            }
            OnSceneDisabled(scene);
        }

        public static void UnloadScene(int buildIndex) => UnloadScene(GetSceneByBuildIndex(buildIndex));

        public static void UnloadScene(string sceneName) => UnloadScene(GetSceneByName(sceneName));

        public static void UnloadScene(Scene scene) => HandleScene(scene, TargetState.Unload);

        private static async Task UnloadSceneAsync(Scene scene)
        {
            switch (scene.LoadingState)
            {
                case LoadingState.Unloaded:
                case LoadingState.Unloading:
                    // Do nothing
                    break;
                case LoadingState.Loading:
                case LoadingState.Loaded:
                case LoadingState.Disabled:
                    if (scene.loadingState is LoadingState.Loading)
                    {
                        await new WaitUntil(() => scene.loadingState is LoadingState.Loaded);
                    }
                    OnSceneUnloading(scene);
                    var asop = USceneManager.UnloadSceneAsync(scene.buildIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                    while (!asop.isDone)
                    {
                        progress = asop.progress;
                        await new WaitForUpdate();
                    }
                    OnSceneUnloaded(scene);
                    break;
                default:
                    break;
            }
        }

        public static void HandleScene(int buildIndex, TargetState targetState) => HandleScene(GetSceneByBuildIndex(buildIndex), targetState);

        public static void HandleScene(string sceneName, TargetState targetState) => HandleScene(GetSceneByName(sceneName), targetState);

        public static void HandleScene(Scene scene, TargetState targetState) => AddProcess(scene, targetState);

        private static void AddProcess(Scene scene, TargetState targetState)
        {
            static async void HandleProcess()
            {
                isBusy = true;
                while (processQueue.Count > 0) // 執行直到佇列中沒有處理對象
                {
                    (var scene, var state) = processQueue.Dequeue();
                    await processMethod[(int)state].Invoke(scene);
                }
                isBusy = false;
            }
            processQueue.Enqueue((scene, targetState));
            if (!isBusy)
            {
                HandleProcess();
            }
        }

        public static Scene GetSceneByName(string sceneName)
        {
            return buildSettingsScenes[sceneName];
        }

        public static Scene GetSceneByBuildIndex(int buildIndex)
        {
            return buildSettingsScenes[buildIndex];
        }

        public static string GetSceneNameByBuildIndex(int buildIndex)
        {
            var path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            return GetSceneNameByScenePath(path);
        }

        public static string GetSceneNameByScenePath(string scenePath)
        {
            // Unity 資產路徑永遠以 '/' 作為分隔
            var start = scenePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
            var end = scenePath.LastIndexOf(".unity", StringComparison.Ordinal);
            return scenePath.Substring(start, end - start);
        }

        internal static KeyList<string, Scene> GetAllBuildSettingsScenes()
        {
            var count = USceneManager.sceneCountInBuildSettings;
            var res = new KeyList<string, Scene>();
            for (int i = 0; i < count; i++)
            {
                var scene = new Scene(i);
                res.Add(scene.sceneName, scene);
            }
            return res;
        }
    }
}