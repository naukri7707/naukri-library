using System;
using UnityEngine.SceneManagement;

namespace Naukri.SceneManagement
{
    public class Scene
    {
        internal LoadingState loadingState;

        public LoadingState LoadingState => loadingState;

        public readonly int buildIndex;

        public readonly string scenePath;

        public readonly string sceneName;

        private Scene() { }

        internal Scene(int buildIndex)
        {
            loadingState = LoadingState.Unloaded;
            this.buildIndex = buildIndex;
            scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            sceneName = SceneManager.GetSceneNameByScenePath(scenePath);
        }
    }
}