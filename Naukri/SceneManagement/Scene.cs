using UnityEngine.SceneManagement;

namespace Naukri.SceneManagement
{
    public delegate void StateChangeEvent(LoadingState oldState, LoadingState newState);

    public class Scene
    {
        private LoadingState loadingState;

        public LoadingState LoadingState
        {
            get => loadingState;
            set
            {
                if (value != loadingState)
                {
                    OnStateChange?.Invoke(loadingState, value);
                    loadingState = value;
                }
            }
        }

        public readonly int buildIndex;

        public readonly string scenePath;

        public readonly string sceneName;

        public event StateChangeEvent OnStateChange;

        private Scene() { }

        public Scene(int buildIndex)
        {
            loadingState = LoadingState.Unloaded;
            this.buildIndex = buildIndex;
            scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            sceneName = SceneManager.GetSceneNameByScenePath(scenePath);
        }
    }
}