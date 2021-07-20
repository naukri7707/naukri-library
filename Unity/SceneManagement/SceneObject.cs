using Naukri;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Naukri.Unity.SceneManagement
{
    [Serializable]
    public struct SceneObject
    {

#if UNITY_EDITOR
        [SerializeField]
        private UnityEngine.Object _sceneAsset;
#endif

        [SerializeField]
        private string _sceneName;

        public string Name => _sceneName;

        public Scene GetScene()
        {
            return USceneManager.GetSceneByName(_sceneName);
        }

        public static implicit operator string(SceneObject sceneObject)
        {
            return sceneObject._sceneName;
        }

        public static implicit operator Scene(SceneObject sceneObject)
        {
            return sceneObject.GetScene();
        }
    }
}