using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NaukriEditor
{
    public abstract class BetterPropertyDrawer : PropertyDrawer
    {
        private bool isInit;

        public bool IsInit => isInit;

        public bool IsGUI => BetterGUILayout.IsGUI;

        public bool IsGetHeight => !BetterGUILayout.IsGUI;

#pragma warning disable IDE1006 // 命名樣式
        public Rect position => BetterGUILayout.renderPosition;
#pragma warning restore IDE1006 // 命名樣式

        public virtual void OnInit(SerializedProperty property, GUIContent label) { }

        public abstract void OnGUILayout(SerializedProperty property, GUIContent label);

        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!isInit)
            {
                OnInit(property, label);
                isInit = true;
            }
            BetterGUILayout.StartGetHeight();
            OnGUILayout(property, label);
            return BetterGUILayout.LayoutHeight;
        }

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BetterGUILayout.StartGUI(position);
            OnGUILayout(property, label);
        }

        public sealed override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return base.CanCacheInspectorGUI(property);
        }

        public sealed override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return base.CreatePropertyGUI(property);
        }
    }
}
