using Naukri.Extensions;
using Naukri.Serializable;
using NaukriEditor.BetterAttribute.Core;
using NaukriEditor.Factory;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NaukriEditor.Serializable
{
    [CustomPropertyDrawer(typeof(SerializableHashSet<>), true)]
    public sealed class SerializableHashSetDrawer : BetterPropertyDrawer
    {
        private static readonly GUIStyle headerStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };

        public ReorderableList reorderableList;

        public GUIStyle transparentStyle;

        private GUIContent displayLabel;

        public override void OnInit(SerializedProperty property, GUIContent label)
        {
            transparentStyle = new GUIStyle
            {
                normal = {background = TextureFactory.SolidColor(new Color32(0, 0, 0, 0))}
            };
            // set display label
            var dataSP = property.FindPropertyRelative("newData").type;
            dataSP = dataSP.Substring("PPtr<$", ">", out var v) ? v : dataSP;
            displayLabel = new GUIContent($"{label.text}  ({dataSP})");
            //
            var valuesSP = property.FindPropertyRelative("values");
            reorderableList = new ReorderableList(valuesSP.serializedObject, valuesSP)
            {
                elementHeight = 20,
                drawHeaderCallback = rect =>
                {
                    if (GUI.Button(rect, "", headerStyle))
                    {
                        property.isExpanded = false;
                    }
                    EditorGUI.LabelField(rect, displayLabel, headerStyle);
                },
                drawElementCallback = (rect, index, selected, focused) =>
                {
                    rect.height -= 2; // 移除 elementHeight 為了讓欄位中間有空間額外加的 2 (18 -> 20)
                    rect.yMin++;      // 讓欄位在 rect 中間渲染
                    var valueSP = valuesSP.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, valueSP);
                },
                onAddCallback = list =>
                {
                    ReorderableList.defaultBehaviours.DoAddButton(list);
                },
                onRemoveCallback = list =>
                {
                    ReorderableList.defaultBehaviours.DoRemoveButton(list);
                },
            };
        }

        public override bool OnGUILayout(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                LayoutWrapper(rect => reorderableList.DoList(rect), reorderableList.GetHeight());
                if (IsGUI)
                {
                    var newDataSP = property.FindPropertyRelative("newData");
                    var rect = position;
                    rect.width -= 70F;
                    rect.yMin = rect.yMax - 18F;
                    EditorGUI.PropertyField(rect, newDataSP);
                }
            }
            else
            {
                BetterGUILayout.PropertyField(property, displayLabel);
            }
            return true;
        }
    }
}
