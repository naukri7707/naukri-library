using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace NaukriEditor
{
    [CustomPropertyDrawer(typeof(DisplayUnityObjectFieldsAttribute))]
    public class DisplayUnityObjectFieldsAttributeDrawer : BetterPropertyDrawer
    {
        private DisplayUnityObjectFieldsAttribute attr;

        private string displayName;

        public override void OnInit(SerializedProperty property, GUIContent label)
        {
            attr = attribute as DisplayUnityObjectFieldsAttribute;
            property.isExpanded = attr.defaultExpanded;
            displayName = attr.name is null ? label.text : attr.name;
        }

        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            if (EditorGUI.indentLevel != 0)
            {
                throw new UnityException($"{nameof(DisplayUnityObjectFieldsAttribute)} 不適用於遞迴結構，這也許是因為目標物件的欄位包含了 {nameof(DisplayUnityObjectFieldsAttribute)} 屬性所導致的");
            }
            label.text = displayName;
            label = EditorGUI.BeginProperty(position, label, property);
            BetterGUILayout.PropertyField(property, label);
            if (IsGUI)
            {
                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, "", true);
            }
            if (property.isExpanded)
            {
                var data = property.objectReferenceValue;
                if (data is null)
                {
                    return;
                }
                var dataSO = new SerializedObject(data);
                var dataSP = dataSO.GetIterator();
                EditorGUI.indentLevel++;
                dataSP.NextVisible(true);
                if (!attr.skipScriptField)
                {
                    BetterGUILayout.ReadOnlyScope(() =>
                    {
                        BetterGUILayout.PropertyField(dataSP);
                    });
                }
                EditorGUI.BeginChangeCheck();
                while (dataSP.NextVisible(false))
                {
                    if (!attr.skipFieldNames.Contains(dataSP.name))
                    {
                        BetterGUILayout.PropertyField(dataSP);
                    }
                }
                if (EditorGUI.EndChangeCheck())
                {
                    dataSP.serializedObject.ApplyModifiedProperties();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }
    }
}
