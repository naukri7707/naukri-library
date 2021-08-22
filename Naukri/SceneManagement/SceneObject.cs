﻿using Naukri;
using Naukri.BetterAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

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

        [SerializeField]
        private TargetState _targetState;

        public TargetState TargetState { get => _targetState; set => _targetState = value; }

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