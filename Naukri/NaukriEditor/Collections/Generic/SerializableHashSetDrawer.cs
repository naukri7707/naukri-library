using Naukri.Extensions;
using Naukri.Collections.Generic;
using NaukriEditor.BetterAttribute.Core;
using NaukriEditor.Factory;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;

namespace NaukriEditor.Collections.Generic
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
                normal = { background = TextureFactory.SolidColor(new Color32(0, 0, 0, 0)) }
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

        public override IEnumerable<BetterGUIWrapper> OnGUILayout(SerializedProperty property, GUIContent label, bool isOnGUI)
        {
            if (property.isExpanded)
            {
                yield return BetterGUILayout.Wrapper(reorderableList.GetHeight(), () => reorderableList.DoList(position));
                if (isOnGUI)
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
                yield return BetterGUILayout.PropertyField(property, displayLabel);
            }
        }
    }
}
