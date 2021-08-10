using Naukri.Extensions;
using Naukri.Unity.Serializable;
using Naukri.UnityEditor.BetterAttribute.Core;
using Naukri.UnityEditor.Factory;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Naukri.UnityEditor.Serializable
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
    public sealed class SerializableDictionaryDrawer : BetterPropertyDrawer
    {
        private const string ArrowText = " ►";

        private static readonly GUIStyle headerStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };

        public ReorderableList reorderableList;

        public GUIStyle transparentStyle;

        public override void OnInit(SerializedProperty property, GUIContent label)
        {
            transparentStyle = new GUIStyle
            {
                normal = { background = TextureFactory.SolidColor(new Color32(0, 0, 0, 0)) }
            };
            // set display label
            var newDataSP = property.FindPropertyRelative("newData");
            var keyType = newDataSP.FindPropertyRelative("key").type;
            keyType = keyType.Substring("PPtr<$", ">", out var k) ? k : keyType;
            var valueType = newDataSP.FindPropertyRelative("value").type;
            valueType = valueType.Substring("PPtr<$", ">", out var v) ? v : keyType;
            label = new GUIContent($"{label.text}  ({keyType}, {valueType})");
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
                    EditorGUI.LabelField(rect, label, headerStyle);
                },
                drawElementCallback = (rect, index, selected, focused) =>
                {
                    var pairSP = valuesSP.GetArrayElementAtIndex(index);
                    var keySP = pairSP.FindPropertyRelative("key");
                    var valueSP = pairSP.FindPropertyRelative("value");
                    const float arrowWidth = 20;
                    var keyWidth = (rect.width - arrowWidth) * 0.3F;
                    var valueWidth = rect.width - keyWidth - arrowWidth;
                    rect.height -= 2; // 移除 elementHeight 為了讓欄位中間有空間額外加的 2 (18 -> 20)
                    rect.yMin++;      // 讓欄位在 rect 中間渲染
                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUI.PropertyField(
                            new Rect(rect.xMin, rect.yMin, keyWidth, rect.height),
                            keySP,
                            GUIContent.none
                        );
                    }
                    EditorGUI.PrefixLabel(
                        new Rect(rect.xMin + keyWidth, rect.yMin, arrowWidth, rect.height),
                        new GUIContent(ArrowText)
                    );
                    EditorGUI.PropertyField(
                        new Rect(rect.xMax - valueWidth, rect.yMin, valueWidth, rect.height),
                        valueSP,
                        GUIContent.none
                    );
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
                LayoutWrapper(() => reorderableList.DoList(position), reorderableList.GetHeight());
                if (IsGUI)
                {
                    var newDataSP = property.FindPropertyRelative("newData");
                    var newKeySP = newDataSP.FindPropertyRelative("key");
                    var newValueSP = newDataSP.FindPropertyRelative("value");
                    var rect = position;
                    rect.width = rect.width - 70F;
                    rect.yMin = rect.yMax - 18F;
                    const float arrowWidth = 20;
                    const float prefixWidth = 65F;

                    var keyWidth = (rect.width - arrowWidth - prefixWidth) * 0.3F;
                    var valueWidth = rect.width - keyWidth - arrowWidth - prefixWidth;
                    EditorGUI.LabelField(
                        new Rect(rect.xMin, rect.yMin, prefixWidth, rect.height),
                        new GUIContent("New Data")
                    );
                    rect.xMin += prefixWidth;
                    EditorGUI.PropertyField(
                        new Rect(rect.xMin, rect.yMin, keyWidth, rect.height),
                        newKeySP, GUIContent.none
                    );
                    rect.xMin += keyWidth;
                    EditorGUI.PrefixLabel(
                        new Rect(rect.xMin, rect.yMin, arrowWidth, rect.height),
                        new GUIContent(ArrowText)
                    );
                    rect.xMin += arrowWidth;
                    EditorGUI.PropertyField(
                        new Rect(rect.xMin, rect.yMin, valueWidth, rect.height),
                        newValueSP, GUIContent.none
                    );
                }
            }
            else
            {
                BetterGUILayout.PropertyField(property, label);
            }
            return true;
        }
    }
}
