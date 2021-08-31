using Naukri.BetterInspector;
using Naukri.BetterInspector.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterInspector.Core
{
    public abstract class BetterInspectorEditor : Editor
    {
        public const BindingFlags bindingAllDeclaredMember = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private static Dictionary<Type, Type> _drawerTypeForType;

        internal static Dictionary<Type, Type> DrawerTypeForType
        {
            get
            {
                if (_drawerTypeForType is null)
                {
                    _drawerTypeForType = CreateDrawerTypeForTypeDictionary();
                }
                return _drawerTypeForType;
            }
        }

        private List<(MemberInfo, InspectorMemberDrawer[])> propertyDrawerCache;

        private List<(MemberInfo, InspectorMemberDrawer[])> methodDrawerCache;

        private bool defaultInspector;

        public bool DisplayFields
        {
            get => BetterInspectorSettings.Instance.displayFields;
            set => BetterInspectorSettings.Instance.displayFields = value;
        }

        public bool DisplayProperties
        {
            get => BetterInspectorSettings.Instance.displayProperties;
            set => BetterInspectorSettings.Instance.displayProperties = value;
        }

        public bool DisplayMethods
        {
            get => BetterInspectorSettings.Instance.displayMethods;
            set => BetterInspectorSettings.Instance.displayMethods = value;
        }

        private static Dictionary<Type, Type> CreateDrawerTypeForTypeDictionary()
        {
            var res = new Dictionary<Type, Type>();
            var drawerTypes = TypeCache.GetTypesDerivedFrom<InspectorMemberDrawer>();

            void AddTypeForType(Type attributeType, Type drawerType)
            {
                if (res.TryGetValue(attributeType, out var existDrawerType))
                {
                    throw new UnityException($"{attributeType.Name} 已經存在繪製器 {existDrawerType.Name} 卻又嘗試使用 {drawerType.Name} 作為其繪製器");
                }
                else
                {
                    res[attributeType] = drawerType;
                }
            }

            foreach (var drawerType in drawerTypes)
            {
                var customDrawer = drawerType.GetCustomAttribute<CustomInspectorDrawer>();
                if (customDrawer != null)
                {
                    if (customDrawer.useForChildren)
                    {
                        var targetDerivedTypes = TypeCache.GetTypesDerivedFrom(customDrawer.targetType);
                        foreach (var derivedType in targetDerivedTypes)
                        {
                            AddTypeForType(derivedType, drawerType);
                        }
                    }
                    AddTypeForType(customDrawer.targetType, drawerType);
                }
            }
            return res;
        }

        private void OnEnable()
        {
            var targetType = target.GetType();
            defaultInspector = targetType.GetCustomAttribute<DefaultInspectorAttribute>() != null;
            if (!defaultInspector)
            {
                InitDrawers();
            }
        }

        public override void OnInspectorGUI()
        {
            if (defaultInspector)
            {
                DrawDefaultInspector();
                return;
            }

            ControlPanel();
            if (DisplayFields)
            {
                BetterGUILayout.LableSeparator("Fields");
                DrawDefaultInspector();
            }
            if (DisplayProperties)
            {
                BetterGUILayout.LableSeparator("Properties");
                DrawDrawers(propertyDrawerCache);
            }
            if (DisplayMethods)
            {
                BetterGUILayout.LableSeparator("Methods");
                DrawDrawers(methodDrawerCache);
            }
        }


        private void ControlPanel()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var toggleBoxWidthOption = GUILayout.Width(12F);
                var spacing = 2;
                EditorGUILayout.LabelField("Display", GUILayout.Width(EditorStyles.label.CalcSize(new GUIContent("Display")).x + spacing));
                GUILayout.FlexibleSpace();
                DisplayFields = EditorGUILayout.Toggle(DisplayFields, toggleBoxWidthOption);
                EditorGUILayout.LabelField(
                    "Field",
                    GUILayout.Width(EditorStyles.label.CalcSize(new GUIContent("Field")).x + spacing)
                    );
                DisplayProperties = EditorGUILayout.Toggle(DisplayProperties, toggleBoxWidthOption);
                EditorGUILayout.LabelField(
                    "Property",
                    GUILayout.Width(EditorStyles.label.CalcSize(new GUIContent("Property")).x + spacing)
                    );
                DisplayMethods = EditorGUILayout.Toggle(DisplayMethods, toggleBoxWidthOption);
                EditorGUILayout.LabelField(
                   "Method",
                   GUILayout.Width(EditorStyles.label.CalcSize(new GUIContent("Method")).x)
                   );
            }
        }

        private void GetInspectedMembers(out List<MemberInfo> propertyInfos, out List<MemberInfo> methodInfos)
        {
            var propertyInfoDict = new Dictionary<string, MemberInfo>();
            var methodInfoDict = new Dictionary<string, MemberInfo>();
            var targetType = target.GetType();
            if (targetType == typeof(UnityEngine.Object))
            {
                propertyInfos = null;
                methodInfos = null;
                return; // 過濾掉 Unity 在按下 Play 鍵後要求繪製用 BetterInspector 繪製 UnityObject 物件的情況
            }
            var inspectedTypes = new List<Type>();
            var customEditorAttr = GetType().GetCustomAttribute<CustomEditor>();
            var inspectedBaseType = typeof(CustomEditor).GetField("m_InspectedType", bindingAllDeclaredMember).GetValue(customEditorAttr) as Type;  // 抓取開始檢查的類別
            var currentType = targetType;
            do
            {
                inspectedTypes.Add(currentType);
                currentType = currentType.BaseType;
            } while (currentType != inspectedBaseType);
            inspectedTypes.Reverse(); // 反轉，從開始檢查的類別開始
            foreach (var inspectedType in inspectedTypes)
            {
                foreach (var info in inspectedType.GetProperties(bindingAllDeclaredMember))
                {
                    propertyInfoDict[info.Name] = info;
                }
                foreach (var info in inspectedType.GetMethods(bindingAllDeclaredMember).Where(it => !it.IsSpecialName))  // 略過 Property 的 getter 和 setter
                {
                    propertyInfoDict[info.Name] = info;
                }
            }
            // 因為 dictionary 沒有刪除過所以會依 key 加入的順序排列
            propertyInfos = propertyInfoDict.Values.ToList();
            methodInfos = methodInfoDict.Values.ToList();
        }

        private List<(MemberInfo, InspectorMemberDrawer[])> BuildDrawerCache(List<MemberInfo> infos)
        {
            var res = new List<(MemberInfo, InspectorMemberDrawer[])>();
            foreach (var info in infos)
            {
                var attrs = info
                    .GetCustomAttributes<PropertyAttribute>(true) // 使用 PropertyAttribute 作為 InspectorAttribute 的基底類別
                    .OrderBy(it => it.order);                     // 讓 BetterInspector 可以自訂 BetterAttribute 的擴充屬性
                // 加入至快取
                var drawers = attrs
                    .Select(attr => GetDrawerTypeForType(attr.GetType()))
                    .Where(drawerType => drawerType != null) // 剔除沒有對應到的 Drawer
                    .Select(drawerType => Activator.CreateInstance(drawerType) as InspectorMemberDrawer)
                    .Where(drawerType => drawerType != null) // 剔除轉型失敗的 Drawer
                    .ToArray();
                if (drawers.Length > 0) // 有 Drawer 才加入
                {
                    res.Add((info, drawers));
                    // 初始化 BetterGUIDrawer
                    foreach (var drawer in drawers)
                    {
                        drawer.Target = target;
                        drawer.memberInfo = info;
                        drawer.OnInit();
                    }
                }
            }
            return res;
        }

        public void InitDrawers()
        {
            GetInspectedMembers(out var propertyInfos, out var methodInfos);
            propertyDrawerCache = BuildDrawerCache(propertyInfos);
            methodDrawerCache = BuildDrawerCache(methodInfos);
        }

        public void DrawDrawers(List<(MemberInfo, InspectorMemberDrawer[])> drawerCache)
        {
            foreach ((var info, var drawers) in drawerCache)
            {
                var displayName = BetterGUILayout.GetDispalyName(info.Name);
                var label = new GUIContent(displayName);
                foreach (var drawer in drawers)
                {
                    drawer.OnBeforeGUILayout(label);
                }
                foreach (var drawer in drawers) // 如果沒有完成繪製，則由下一優先序 Drawer 繪製直到繪製完成
                {
                    if (drawer.OnGUILayout(label))
                    {
                        break;
                    }
                }
                foreach (var drawer in drawers.Reverse())
                {
                    drawer.OnAfterGUILayout(label);
                }
            }
        }

        public static Type GetDrawerTypeForType<T>()
        => GetDrawerTypeForType(typeof(T));

        public static Type GetDrawerTypeForType(Type type)
        {
            return DrawerTypeForType[type];
        }
    }
}