using Naukri;
using Naukri.BetterAttribute;
using Naukri.BetterInspector;
using Naukri.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Naukri.SceneManagement
{
    public class HandleScenes : NaukriBehaviour
    {
        [ExpandElement]
        public SceneObject[] targetScenes;

        private void Start()
        {
            foreach (var scene in targetScenes)
            {
                SceneManager.HandleScene(scene.Name, scene.TargetState);
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadScene(sceneName);
        }

        public void DisableScene(string sceneName)
        {
            SceneManager.DisableScene(sceneName);
        }

        [DisplayMethod]
        public void HandleScene(string sceneName, TargetState targetState)
        {
            SceneManager.HandleScene(sceneName, targetState);
        }
    }
}