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
        private UnityEngine.Object sceneAsset;
#endif

        [SerializeField]
        private string sceneName = "";

        public string Name => sceneName;

        public static implicit operator string(SceneObject sceneField)
        {
            return sceneField.sceneName;
        }
    }
}