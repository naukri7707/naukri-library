using UnityEngine;
using UnityEditor;
using Naukri.SceneManagement;

namespace NaukriEditor.Helper
{
    public class SceneManagerWindow : EditorWindow
    {
        [MenuItem("Naukri/Tools/Scene Manager")]
        public static void ShowWindow()
        {
            var window = GetWindow<SceneManagerWindow>("Scene Manager");
        }

        private enum Tab { Scenes, Process }

        private Tab tab;

        public void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField($"This window only works in play mode");
                return;
            }
            var tabTitles = new[]
            {
                Tab.Scenes.ToString(),
                $"{Tab.Process} ({SceneManager.ProcessQueue.Count})"
            };

            tab = (Tab)GUILayout.Toolbar((int)tab, tabTitles);
            var repaint = tab switch
            {
                Tab.Scenes => DrawScenes(),
                Tab.Process => DrawProcess(),
                _ => throw new System.NotImplementedException()
            };
            if (repaint)
            {
                Repaint();
            }
        }

        private bool DrawProcess()
        {
            var processQueue = SceneManager.ProcessQueue;
            if (processQueue.Count > 0)
            {
                foreach ((var scene, var targetState) in processQueue)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        var title = new GUIContent()
                        {
                            text = $"{scene.sceneName}...{(int)(SceneManager.Progress * 100)}%",
                            tooltip = scene.scenePath
                        };
                        var state = (LoadingState)EditorGUILayout.EnumPopup(title, targetState);
                    }
                }
            }
            return true;
        }

        private bool DrawScenes()
        {
            var scenes = SceneManager.BuildSettingsScenes;
            var padLeft = (scenes.Count - 1).ToString().Length;
            foreach (var scene in scenes)
            {
                using (new EditorGUI.DisabledScope(SceneManager.IsBusy))
                {
                    var title = new GUIContent()
                    {
                        text = $"{scene.buildIndex.ToString().PadLeft(padLeft)}. {scene.sceneName}",
                        tooltip = scene.scenePath
                    };
                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        var state = (LoadingState)EditorGUILayout.EnumPopup(title, scene.LoadingState);
                        if (check.changed)
                        {
                            switch (state)
                            {
                                case LoadingState.Unloaded:
                                case LoadingState.Unloading:
                                    SceneManager.UnloadScene(scene);
                                    break;
                                case LoadingState.Loaded:
                                case LoadingState.Loading:
                                    SceneManager.LoadScene(scene);
                                    break;
                                case LoadingState.Disabled:
                                    SceneManager.DisableScene(scene);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (SceneManager.IsBusy)
                    {
                        EditorGUILayout.LabelField("Processing...");
                    }
                }
            }
            return true;
        }

    }
}