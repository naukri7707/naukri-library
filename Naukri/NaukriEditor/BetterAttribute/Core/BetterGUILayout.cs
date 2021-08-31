using System;
using UnityEditor;
using UnityEngine;
using static NaukriEditor.BetterAttribute.Core.BetterPropertyDrawer;

namespace NaukriEditor.BetterAttribute.Core
{
    public static class BetterGUILayout
    {
        public const float PropertySpace = 2F;

        public const float PropertyHeight = 18F;

        public static BetterGUIWrapper Wrapper(DrawEvent drawEvent) => new BetterGUIWrapper(drawEvent);

        public static BetterGUIWrapper Wrapper(float height, DrawEvent drawEvent) => new BetterGUIWrapper(height, drawEvent);

        public static BetterGUIWrapper EmptyField(float height) => Wrapper(height, () => { });

        #region -- PropertyFields --

        public static BetterGUIWrapper PropertyField(SerializedProperty property)
            => PropertyField(property, new GUIContent(property.displayName), false);

        public static BetterGUIWrapper PropertyField(SerializedProperty property, bool includeChildren)
            => PropertyField(property, new GUIContent(property.displayName), includeChildren);

        public static BetterGUIWrapper PropertyField(SerializedProperty property, GUIContent label)
            => PropertyField(property, label, false);

        public static BetterGUIWrapper PropertyField(SerializedProperty property, GUIContent label, bool includeChildren)
        {
            return Wrapper(EditorGUI.GetPropertyHeight(property), () => EditorGUI.PropertyField(position, property, label, includeChildren));
        }

        #endregion

        #region -- ButtonFields --

        public static BetterGUIWrapper ButtonField(DrawResult<bool> onResult, GUIContent label, GUIContent value, GUIStyle style = null)
        {
            return ButtonField(onResult, label, value, out var _, out var _, style);
        }

        public static BetterGUIWrapper ButtonField(DrawResult<bool> onResult, GUIContent label, GUIContent value, out Rect fieldRect, GUIStyle style = null)
        {
            return ButtonField(onResult, label, value, out fieldRect, out var _, style);
        }

        public static BetterGUIWrapper ButtonField(DrawResult<bool> onResult, GUIContent label, GUIContent value, out Rect fieldRect, out Rect controlRect, GUIStyle style = null)
        {
            var _fieldRect = Rect.zero;
            var _controlRect = Rect.zero;
            // 因 Wrapper 在 OnGUI 時必定立即觸發 DrawEvent 所以可以在裡面賦值不會有問題
            var res = Wrapper(() =>
           {
               _fieldRect = position;
               _controlRect = EditorGUI.PrefixLabel(position, label);
               onResult(GUI.Button(_controlRect, value, new GUIStyle(style ?? GUI.skin.button)));
           });
            // 這裡只有在 OnGUI 時才會被正確賦值
            fieldRect = _fieldRect;
            controlRect = _controlRect;
            return res;
        }

        #endregion

        public static BetterGUIWrapper SeparatorLine(float thickness, float space)
        {
            if (thickness > space) thickness = space;
            return Wrapper(space, () =>
            {
                var rect = position;
                rect.y = (rect.yMin + rect.yMax) / 2 - thickness / 2;
                rect.height = thickness;
                EditorGUI.DrawRect(rect, new Color(0.5F, 0.5F, 0.5F, 1));
            });
        }
    }
}
