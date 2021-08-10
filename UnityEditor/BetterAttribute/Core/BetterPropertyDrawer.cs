using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Naukri.UnityEditor.BetterAttribute.Core
{
    [InitializeOnLoad]
    public abstract class BetterPropertyDrawer : PropertyDrawer
    {
        private const float SPACING = 2F;

        public const BindingFlags binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private static readonly MethodInfo _getDrawerTypeForTypeMethodInfo = Type
            .GetType("UnityEditor.ScriptAttributeUtility, UnityEditor.CoreModule")
            .GetMethod("GetDrawerTypeForType", binding);

        public static BetterPropertyDrawer currentFramework;

        private BetterPropertyDrawer[] drawers;

        public bool IsInit { get; private set; }

        private bool isFramework;

        // 只作用於 Framework 的變數，在 Drawer 上用存取子抓 Framework 裡的變數
        private bool _isFirst;

        private bool _isGUI;

        private float _height;

        private Rect _position;
        //

#pragma warning disable IDE1006 // 命名樣式

        public new PropertyAttribute attribute { get; private set; }

        public Rect position // 讓 Drawer 可以讀取到框架的 position
        {
            get => currentFramework._position;
            set => currentFramework._position = value;
        }

#pragma warning restore IDE1006 // 命名樣式


        public bool IsGetHeight => !IsGUI;

        public bool IsFirst
        {
            get => currentFramework._isFirst;
            private set => currentFramework._isFirst = value;
        }
        public bool IsGUI
        {
            get => currentFramework._isGUI;
            private set => currentFramework._isGUI = value;
        }
        public float Height
        {
            get => currentFramework._height;
            private set => currentFramework._height = value;
        }

        public virtual void OnInit(SerializedProperty property, GUIContent label) { }

        public virtual bool OnGUILayout(SerializedProperty property, GUIContent label)
        {
            return false;
        }

        public virtual void OnBeforeGUILayout(SerializedProperty property, GUIContent label) { }
        public virtual void OnAfterGUILayout(SerializedProperty property, GUIContent label) { }

        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!IsInit)
            {
                var subDrawers = new List<BetterPropertyDrawer>();
                isFramework = true;
                attribute = base.attribute;
                OnInit(property, label);
                var attrs = fieldInfo
                        .GetCustomAttributes<PropertyAttribute>(true)
                        .OrderBy(it => it.order)
                        .ToArray();
                foreach (var attr in attrs)
                {
                    var drawerType = GetDrawerTypeForType(attr.GetType());
                    if (drawerType is null)
                        continue;
                    if (Activator.CreateInstance(drawerType) is BetterPropertyDrawer subDrawer)
                    {
                        // 初始化 Drawer
                        subDrawer.attribute = attr;
                        subDrawer.OnInit(property, label);
                        subDrawer.IsInit = true;
                        subDrawers.Add(subDrawer);
                    }
                }
                drawers = subDrawers.ToArray();
                IsInit = true;
            }
            if (isFramework)
            {
                //
                _height = 0F;
                _isFirst = true;
                _isGUI = false;
                //
                var parentFramework = currentFramework;
                currentFramework = this;
                DoGUILayout(property, label);
                currentFramework = parentFramework;
                return _height;
            }
            return 0;
        }

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (isFramework)
            {
                position.height = 0F;
                _position = position;
                _isFirst = true;
                _isGUI = true;
                //
                var parentFramework = currentFramework;
                currentFramework = this;
                DoGUILayout(property, label);
                currentFramework = parentFramework;
            }
        }

        public void DoGUILayout(SerializedProperty property, GUIContent label)
        {
            // 因為在複數個 Attribute 的情況下 unity editor 無法利用 order 抓取最優先項
            // 故在讀取到首項後生成全部 attribute 的對應 drawer (包含和首項相同的 drawer) 並排序之
            // 再使用生成出來的 drawer 進行繪製 / 高度測算
            // 所以由 unity editor 所生成的首項只做為展開框架使用，不參與實際繪製 / 測算
            label = new GUIContent(property.displayName);
            foreach (var drawer in drawers)
            {
                drawer.OnBeforeGUILayout(property, label);
            }
            var isDrawed = false;
            foreach (var drawer in drawers) // 如果沒有完成繪製，則由下一優先序 Drawer 繪製直到繪製完成
            {
                isDrawed = drawer.OnGUILayout(property, label);
                if (isDrawed)
                {
                    break;
                }
            }
            if (!isDrawed) // 如果都沒有完成繪製，繪製預設欄位
            {
                BetterGUILayout.PropertyField(property, label);
            }
            foreach (var drawer in drawers.Reverse())
            {
                drawer.OnAfterGUILayout(property, label);
            }
        }

        public sealed override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return base.CreatePropertyGUI(property);
        }

        public void LayoutWrapper(Action drawer, float height, float spacing = SPACING)
        {
            if (IsFirst)
            {
                spacing = 0;
                IsFirst = false;
            }

            if (IsGUI)
            {
                var pos = position;
                pos.yMin = pos.yMax + spacing; // StartLayout 後因 height 為 0， 故首行 pos.yMax 為 yMin
                pos.height = height;                 // 在渲染/計算前才將 yMin 移動到下一個欄位應該出現的 yMin 上，避免 position 出現錯位
                position = pos;
                drawer.Invoke();
            }
            else
            {
                Height += height + spacing;
            }
        }

        public void LayoutWrapper<T>(ref T target, Func<T> drawer, float height, float spacing = SPACING)
        {
            if (IsFirst)
            {
                spacing = 0;
                IsFirst = false;
            }

            if (IsGUI)
            {
                var pos = position;
                pos.yMin = pos.yMax + spacing; // StartLayout 後因 height 為 0， 故首行 pos.yMax 為 yMin
                pos.height = height;                 // 在渲染/計算前才將 yMin 移動到下一個欄位應該出現的 yMin 上，避免 position 出現錯位
                position = pos;
                target = drawer.Invoke();
            }
            else
            {
                Height += height + spacing;
            }
        }

        private static Type GetDrawerTypeForType<T>()
            => GetDrawerTypeForType(typeof(T));

        private static Type GetDrawerTypeForType(Type type)
        {
            return _getDrawerTypeForTypeMethodInfo
                .Invoke(null, new[] { type }) as Type;
        }
    }
}
