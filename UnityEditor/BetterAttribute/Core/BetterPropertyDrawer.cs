using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Naukri.UnityEditor.BetterAttribute.Core
{
    [InitializeOnLoad]
    public abstract class BetterPropertyDrawer : PropertyDrawer
    {
        private const float SPACING = 2F;

        private const int MAX_STACK_COUNT = 7;

        private static readonly Stack<BetterPropertyDrawer> drawerStack;

        public static BetterPropertyDrawer CurrentDrawer
            => drawerStack is null || drawerStack.Count is 0 ? null : drawerStack.Peek();

        static BetterPropertyDrawer()
        {
            drawerStack = new Stack<BetterPropertyDrawer>();
        }

        public bool IsInit { get; private set; }

        private bool isFirst;

        private float height;

        private Rect _position;

#pragma warning disable IDE1006 // 命名樣式
        public Rect position => _position;

#pragma warning restore IDE1006 // 命名樣式

        public bool IsGUI { get; private set; }

        public bool IsGetHeight => !IsGUI;

        public virtual void OnInit(SerializedProperty property, GUIContent label) { }

        public abstract void OnGUILayout(SerializedProperty property, GUIContent label);

        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!IsInit)
            {
                OnInit(property, label);
                IsInit = true;
            }
            //
            height = 0F;
            isFirst = true;
            IsGUI = false;
            drawerStack.Push(this);
            CheckStackDepth();
            //
            OnGUILayout(property, label);
            //
            drawerStack.Pop();
            return height;
        }

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = 0F;
            _position = position;
            isFirst = true;
            IsGUI = true;
            drawerStack.Push(this);
            CheckStackDepth();
            //
            OnGUILayout(property, label);
            //
            drawerStack.Pop();
        }

        public sealed override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return base.CanCacheInspectorGUI(property);
        }

        public sealed override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return base.CreatePropertyGUI(property);
        }

        public void LayoutContainer(Action drawer, float height, float spacing = SPACING)
        {
            if (isFirst)
            {
                spacing = 0;
                isFirst = false;
            }

            if (IsGUI)
            {
                _position.yMin = _position.yMax + spacing; // StartLayout 後因 height 為 0， 故首行 pos.yMax 為 yMin
                _position.height = height;                // 在渲染/計算前才將 yMin 移動到下一個欄位應該出現的 yMin 上，避免 position 出現錯位
                drawer.Invoke();
            }
            else
            {
                this.height += height + spacing;
            }
        }

        public void LayoutContainer<T>(ref T target, Func<T> drawer, float height, float spacing = SPACING)
        {
            if (isFirst)
            {
                spacing = 0;
                isFirst = false;
            }

            if (IsGUI)
            {
                _position.yMin = _position.yMax + spacing; // StartLayout 後因 height 為 0， 故首行 pos.yMax 為 yMin
                _position.height = height;                // 在渲染/計算前才將 yMin 移動到下一個欄位應該出現的 yMin 上，避免 position 出現錯位
                target = drawer.Invoke();
            }
            else
            {
                this.height += height + spacing;
            }
        }

        private static void CheckStackDepth()
        {
            if (drawerStack.Count > MAX_STACK_COUNT)
            {
                throw new UnityException($"堆疊深度過深，深度最大不可超過 {MAX_STACK_COUNT}");
            }
        }
    }
}
