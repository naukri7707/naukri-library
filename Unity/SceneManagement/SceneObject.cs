using Naukri;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naukri.Unity.SceneManagement
{
    [Serializable]
    public class SceneObject
    {

#if UNITY_EDITOR
        [SerializeField]
        private UnityEngine.Object _sceneAsset;
#endif

        [SerializeField]
        private string _sceneName = "";

        public string Name => _sceneName;

        public static implicit operator string(SceneObject sceneObject)
        {
            return sceneObject._sceneName;
        }
    }
}