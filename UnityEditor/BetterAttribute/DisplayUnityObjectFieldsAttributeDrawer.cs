using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using Naukri.Unity.BetterAttribute;

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
            label.text = displayName;
            label = BetterGUILayout.BeginProperty(label, property);
            BetterGUILayout.PropertyField(property, label);
            if (property.propertyType != SerializedPropertyType.ObjectReference) // 若母標不是 UnityObject 則單純視為 PropertyField
            {
                BetterGUILayout.EndProperty();
                return;
            }
            if (IsGUI)
            {
                property.isExpanded = BetterGUILayout.Foldout(property.isExpanded, "", true);
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
                BetterGUILayout.BeginChangeCheck();
                while (dataSP.NextVisible(false))
                {
                    if (!attr.skipFieldNames.Contains(dataSP.name))
                    {
                        BetterGUILayout.PropertyField(dataSP);
                    }
                }
                if (BetterGUILayout.EndChangeCheck())
                {
                    dataSP.serializedObject.ApplyModifiedProperties();
                }
                EditorGUI.indentLevel--;
            }
            BetterGUILayout.EndProperty();
        }
    }
}
