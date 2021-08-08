using Naukri;
using Naukri.Unity.BetterAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Naukri.Unity.SceneManagement
{

    [Serializable]
    public struct SceneObject : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField, PropertyUsage(typeof(UnityEditor.SceneAsset))]
        private UnityEngine.Object _sceneAsset;
#endif

        [SerializeField, HideInInspector]
        private string _sceneName;

        public string Name => _sceneName;

        [SerializeField]
        private LoadingMode _loadingMode;

        public LoadingMode LoadingMode { get => _loadingMode; set => _loadingMode = value; }

        public static implicit operator string(SceneObject sceneObject)
        {
            return sceneObject._sceneName;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
#if UNITY_EDITOR
            _sceneName = _sceneAsset == null ? "" : _sceneAsset.name;
#endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {

        }
    }
}