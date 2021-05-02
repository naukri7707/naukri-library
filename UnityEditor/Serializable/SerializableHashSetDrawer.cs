using Naukri;
using Naukri.Extensions;
using Naukri.Unity.Serializable;
using UnityEngine;
using UnityEditor;
using NaukriEditor;
using UnityEditorInternal;
using NaukriEditor.Factory;

[CustomPropertyDrawer(typeof(SerializableHashSet<>), true)]
public sealed class SerializableHashSetDrawer : BetterPropertyDrawer
{
    private static readonly GUIStyle headerStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };

    public ReorderableList reorderableList;

    public GUIStyle transparentStyle;

    private GUIContent displayLabel;

    public override void OnInit(SerializedProperty property, GUIContent label)
    {
        transparentStyle = new GUIStyle();
        transparentStyle.normal.background = TextureFactory.SolidColor(new Color32(0, 0, 0, 0));
        // set display label
        var dataSP = property.FindPropertyRelative("newData").type;
        dataSP = dataSP.Substring("PPtr<$", ">", out var v) ? v : dataSP;
        displayLabel = new GUIContent($"{label.text}  ({dataSP})");
        //
        var valuesSP = property.FindPropertyRelative("values");
        reorderableList = new ReorderableList(valuesSP.serializedObject, valuesSP)
        {
            drawHeaderCallback = (Rect rect) =>
            {
                if (GUI.Button(rect, "", headerStyle))
                {
                    property.isExpanded = false;
                }
                EditorGUI.LabelField(rect, displayLabel, headerStyle);
            },
            drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
            {
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

    public override void OnGUILayout(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            LayoutContainer(() => reorderableList.DoList(position), reorderableList.GetHeight());
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
    }
}
