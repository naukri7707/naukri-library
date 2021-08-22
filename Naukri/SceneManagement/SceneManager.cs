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
    public static class SceneManager
    {
        private const string DISABLE_ROOT_NAME = "[Disabled]";

        private static readonly Queue<(Scene, TargetState)> processQueue = new Queue<(Scene, TargetState)>();

        private static readonly KeyList<string, Scene> scenes;

        private static readonly object processLock = new object();

        private static float progress;

        public static float Progress => progress;

        static SceneManager()
        {
            scenes = GetAllBuildSettingsScenes();
            scenes[0].LoadingState = LoadingState.Loaded;
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
            switch (scene.LoadingState)
            {
                case LoadingState.Unloaded:
                case LoadingState.Unloading:
                    if (scene.LoadingState is LoadingState.Unloading)
                    {
                        await new WaitUntil(() => scene.LoadingState is LoadingState.Unloaded);
                    }
                    scene.LoadingState = LoadingState.Loading;
                    var asop = USceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Additive);
                    while (!asop.isDone)
                    {
                        progress = asop.progress;
                        await new WaitForUpdate();
                    }
                    scene.LoadingState = LoadingState.Loaded;
                    break;
                case LoadingState.Loading:
                case LoadingState.Loaded:
                    // Do nothing
                    break;
                case LoadingState.Disabled:
                    var uscene = USceneManager.GetSceneAt(scene.buildIndex);
                    var disabled = uscene.GetRootGameObjects()
                        .FirstOrDefault(it => it.name != DISABLE_ROOT_NAME);
                    if (disabled)
                    {
                        foreach (Transform child in disabled.transform)
                        {
                            child.parent = null;
                        }
                        GameObject.Destroy(disabled);
                    }
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
            if (scene.LoadingState is LoadingState.Disabled) return;
            await LoadSceneAsync(scene);
            var uscene = USceneManager.GetSceneAt(scene.buildIndex);
            var disabled = new GameObject(DISABLE_ROOT_NAME);
            disabled.SetActive(false);
            USceneManager.MoveGameObjectToScene(disabled, uscene);
            foreach (GameObject go in uscene.GetRootGameObjects())
            {
                go.transform.SetParent(disabled.transform, false);
            }
            scene.LoadingState = LoadingState.Disabled;
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
                    if (scene.LoadingState is LoadingState.Loading)
                    {
                        await new WaitUntil(() => scene.LoadingState is LoadingState.Loaded);
                    }
                    scene.LoadingState = LoadingState.Unloading;
                    var asop = USceneManager.UnloadSceneAsync(scene.buildIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                    while (!asop.isDone)
                    {
                        progress = asop.progress;
                        await new WaitForUpdate();
                    }
                    scene.LoadingState = LoadingState.Unloaded;
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
                while (processQueue.Count > 0) // 執行直到佇列中沒有處理對象
                {
                    (var scene, var state) = processQueue.Dequeue();
                    await processMethod[(int)state].Invoke(scene);
                }
            }
            processQueue.Enqueue((scene, targetState));
            lock (processLock)
            {
                HandleProcess();
            }
        }

        public static Scene GetSceneByName(string sceneName)
        {
            return scenes[sceneName];
        }

        public static Scene GetSceneByBuildIndex(int buildIndex)
        {
            return scenes[buildIndex];
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