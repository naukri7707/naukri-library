using System;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.BetterAttribute.Core
{
    public static class BetterGUILayout
    {
        #region -- PropertyFields --

        public static bool PropertyField(SerializedProperty property)
            => PropertyField(property, null, false);

        public static bool PropertyField(SerializedProperty property, bool includeChildren)
            => PropertyField(property, null, includeChildren);

        public static bool PropertyField(SerializedProperty property, GUIContent label)
            => PropertyField(property, label, false);

        public static bool PropertyField(SerializedProperty property, GUIContent label, bool includeChildren)
        {
            var res = false;
            BetterPropertyDrawer.CurrentDrawer.LayoutContainer(
                ref res,
                () => EditorGUI.PropertyField(BetterPropertyDrawer.CurrentDrawer.position, property, label, includeChildren),
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
            return EditorGUI.Foldout(BetterPropertyDrawer.CurrentDrawer.position, foldout, content, toggleOnLabelClick, style);
        }

        #endregion

        #region -- Other --

        public static GUIContent BeginProperty(GUIContent label, SerializedProperty property)
            => EditorGUI.BeginProperty(BetterPropertyDrawer.CurrentDrawer.position, label, property);

        public static void EndProperty() => EditorGUI.EndProperty();

        public static void BeginChangeCheck() => EditorGUI.BeginChangeCheck();

        public static bool EndChangeCheck() => EditorGUI.EndChangeCheck();

        #endregion

        #region -- Scopes --

        public static void ReadOnlyScope(Action gui)
        {
            var active = GUI.enabled;
            GUI.enabled = false;
            gui();
            GUI.enabled = active;
        }

        #endregion
    }
}
