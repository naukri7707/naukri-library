using Naukri;
using Naukri.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NaukriEditor.BetterAttribute.Core
{
    public abstract class BetterPropertyDrawer : PropertyDrawer
    {
        private IEnumerator<float> Space()
        {
            yield return 0F;
            for (; ; ) yield return BetterGUILayout.PropertySpace;
        }

        public const BindingFlags binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private static readonly MethodInfo _getDrawerTypeForTypeMethodInfo = Type
            .GetType("UnityEditor.ScriptAttributeUtility, UnityEditor.CoreModule")
            .GetMethod("GetDrawerTypeForType", binding);

        private BetterPropertyDrawer[] drawers;

#pragma warning disable IDE1006 // 命名樣式

        private static Rect _position;

        public static Rect position => _position;

        public new PropertyAttribute attribute { get; private set; }

#pragma warning restore IDE1006 // 命名樣式

        public virtual void OnInit(SerializedProperty property, GUIContent label) { }

        public virtual void OnBeforeGUILayout(SerializedProperty property, GUIContent label) { }

        public virtual IEnumerable<BetterGUIWrapper> OnGUILayout(SerializedProperty property, GUIContent label, bool isOnGUI) { yield break; }

        public virtual void OnAfterGUILayout(SerializedProperty property, GUIContent label) { }

        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (isFramworking) return EditorGUI.GetPropertyHeight(property);
            using (Scope.SetValue(() => ref isFramworking, true, false))
            {
                CreateSubDrawersIfNull(property, label);
                var isDrawed = false;
                var height = 0F;
                var spaceHeight = Space();
                foreach (var drawer in drawers)
                {
                    foreach (var field in drawer.OnGUILayout(property, label, false))
                    {
                        isDrawed = true;
                        height += field.height + spaceHeight.NextValue();
                    }
                    if (isDrawed) return height;
                }
            }
            return BetterGUILayout.PropertyHeight;
        }

        private static bool isFramworking;

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (isFramworking)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            if (drawers is null) return;
            using (Scope.SetValue(() => ref isFramworking, true, false))
            {
                _position = position;
                _position.height = BetterGUILayout.PropertyHeight;
                var isDrawed = false;
                var spaceHeight = Space();
                label = new GUIContent(property.displayName);
                foreach (var drawer in drawers)
                {
                    drawer.OnBeforeGUILayout(property, label);
                }
                foreach (var drawer in drawers) // 如果沒有完成繪製，則嘗試繪製下一優先序的 Drawer 直到繪製完成
                {
                    foreach (var wrapper in drawer.OnGUILayout(property, label, true))
                    {
                        isDrawed = true;
                        DrawWrapper(wrapper, spaceHeight.NextValue());
                    }
                    if (isDrawed) break;
                }
                // 如果都沒有完成繪製，繪製預設欄位
                if (!isDrawed)
                {
                    var wrapper = BetterGUILayout.PropertyField(property, label);
                    DrawWrapper(wrapper, spaceHeight.NextValue());
                }
                foreach (var drawer in drawers.Reverse())
                {
                    drawer.OnAfterGUILayout(property, label);
                }
            }
        }

        private void DrawWrapper(BetterGUIWrapper wrapper, float space)
        {
            _position.height = wrapper.height;
            wrapper.drawEvent();
            _position.yMin = _position.yMax + space;
        }

        public sealed override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return base.CreatePropertyGUI(property);
        }

        private void CreateSubDrawersIfNull(SerializedProperty property, GUIContent label)
        {
            // 因為在複數個 Attribute 的情況下 unity editor 無法利用 order 抓取最優先項
            // 故在讀取到首項後生成全部 attribute 的對應 drawer (包含和首項相同的 drawer) 並排序之
            // 再使用生成出來的 drawer 進行繪製 / 高度測算
            // 所以由 unity editor 所生成的首項只做為展開框架使用，不參與實際繪製 / 測算
            if (drawers != null) return;
            var subDrawers = new List<BetterPropertyDrawer>();
            var attrs = fieldInfo // 取得所有 Drawer 標籤
                    .GetCustomAttributes<PropertyAttribute>(true)
                    .OrderBy(it => -it.order)
                    .ToArray();
            foreach (var attr in attrs)
            {
                var drawerType = GetDrawerTypeForType(attr.GetType());
                if (drawerType is null) continue;
                if (Activator.CreateInstance(drawerType) is BetterPropertyDrawer subDrawer)
                {
                    // 初始化 subDrawer
                    subDrawer.attribute = attr;
                    subDrawer.OnInit(property, label);
                    subDrawers.Add(subDrawer);
                }
            }
            drawers = subDrawers.ToArray();
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
