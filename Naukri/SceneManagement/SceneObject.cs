using Naukri.BetterAttribute;
using System;
using UnityEngine;

namespace Naukri.SceneManagement
{
    [Serializable]
    public struct SceneObject : ISerializationCallbackReceiver
    {

#if UNITY_EDITOR
        [SerializeField, CustomObjectField(typeof(UnityEditor.SceneAsset))]
        private UnityEngine.Object _sceneAsset;
#endif

        [SerializeField, HideInInspector]
        private string _sceneName;

        public string Name => _sceneName;

        public static implicit operator string(SceneObject sceneObject)
        {
            return sceneObject.Name;
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