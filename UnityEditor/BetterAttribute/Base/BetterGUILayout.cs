using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor
{
    public static class BetterGUILayout
    {
        private const float SPACING = 2F;

        private static bool isGUI;

        internal static bool IsGUI => isGUI;

        private static float layoutHeight;

        public static float LayoutHeight => layoutHeight;

        public static Rect renderPosition;

        public static Rect RenderPosition => renderPosition;

        private static bool isFirstField;

        #region -- PropertyFields --

        public static bool PropertyField(SerializedProperty property)
            => PropertyField(property, null, false);

        public static bool PropertyField(SerializedProperty property, bool includeChildren)
            => PropertyField(property, null, includeChildren);

        public static bool PropertyField(SerializedProperty property, GUIContent label)
            => PropertyField(property, label, false);

        public static bool PropertyField(SerializedProperty property, GUIContent label, bool includeChildren)
        {
            return LayoutContainer(
                () => EditorGUI.PropertyField(renderPosition, property, label, includeChildren),
                EditorGUI.GetPropertyHeight(property)
                );
        }

        #endregion

        public static void LayoutContainer(Action drawer, float height, float spacing = SPACING)
        {
            if (isFirstField)
            {
                spacing = 0;
                isFirstField = false;
            }

            if (isGUI)
            {
                renderPosition.yMin = renderPosition.yMax + spacing; // StartLayout 後因 height 為 0， 故首行 pos.yMax 為 yMin
                renderPosition.height = height;                      // 在渲染/計算前才將 yMin 移動到下一個欄位應該出現的 yMin 上，避免 position 出現錯位
                drawer.Invoke();
            }
            else
            {
                layoutHeight += height + spacing;
            }
        }

        public static T LayoutContainer<T>(Func<T> drawer, float height, float spacing = SPACING)
        {
            if (isFirstField)
            {
                spacing = 0;
                isFirstField = false;
            }

            if (isGUI)
            {
                renderPosition.yMin = renderPosition.yMax + spacing; // StartLayout 後因 height 為 0， 故首行 pos.yMax 為 yMin
                renderPosition.height = height;                      // 在渲染/計算前才將 yMin 移動到下一個欄位應該出現的 yMin 上，避免 position 出現錯位
                return drawer.Invoke();
            }
            else
            {
                layoutHeight += height + spacing;
                return default;
            }
        }

        #region -- Scopes --

        public static void ReadOnlyScope(Action gui)
        {
            var active = GUI.enabled;
            GUI.enabled = false;
            gui();
            GUI.enabled = active;
        }

        #endregion


        internal static void StartGetHeight()
        {
            isFirstField = true;
            layoutHeight = 0F;
            isGUI = false;
        }

        internal static void StartGUI(Rect position)
        {
            isFirstField = true;
            position.height = 0F;
            renderPosition = position;
            isGUI = true;
        }
    }
}
