using System;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.BetterAttribute.Core
{
    public static class BetterGUILayout
    {
        #region -- PropertyFields --

        public static bool PropertyField(SerializedProperty property)
            => PropertyField(property, new GUIContent(property.displayName), false);

        public static bool PropertyField(SerializedProperty property, bool includeChildren)
            => PropertyField(property, new GUIContent(property.displayName), includeChildren);

        public static bool PropertyField(SerializedProperty property, GUIContent label)
            => PropertyField(property, label, false);

        public static bool PropertyField(SerializedProperty property, GUIContent label, bool includeChildren)
        {
            var res = false;
            BetterPropertyDrawer.currentFramework.LayoutWrapper(
                ref res,
                () => EditorGUI.PropertyField(BetterPropertyDrawer.currentFramework.position, property, label, includeChildren),
                EditorGUI.GetPropertyHeight(property)
                );
            return res;
        }

        #endregion

        #region -- Foldout --

        public static bool Foldout(bool foldout, string content)
        {
            return Foldout(foldout, new GUIContent(content), false, EditorStyles.foldout);
        }

        public static bool Foldout(bool foldout, string content, GUIStyle style)
        {
            return Foldout(foldout, new GUIContent(content), false, style);
        }

        public static bool Foldout(bool foldout, string content, bool toggleOnLabelClick)
        {
            return Foldout(foldout, new GUIContent(content), toggleOnLabelClick, EditorStyles.foldout);
        }

        public static bool Foldout(bool foldout, string content, bool toggleOnLabelClick, GUIStyle style)
        {
            return Foldout(foldout, new GUIContent(content), toggleOnLabelClick, style);
        }

        public static bool Foldout(bool foldout, GUIContent content)
        {
            return Foldout(foldout, content, false, EditorStyles.foldout);
        }

        public static bool Foldout(bool foldout, GUIContent content, GUIStyle style)
        {
            return Foldout(foldout, content, false, style);
        }

        public static bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick)
        {
            return Foldout(foldout, content, toggleOnLabelClick, EditorStyles.foldout);
        }

        public static bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style)
        {
            return EditorGUI.Foldout(BetterPropertyDrawer.currentFramework.position, foldout, content, toggleOnLabelClick, style);
        }

        #endregion
    }
}
