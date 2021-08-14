﻿using System.Linq;
using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(DisplayObjectFieldsAttribute))]
    public class DisplayObjectFieldsDrawer : BetterPropertyDrawer
    {
        private DisplayObjectFieldsAttribute attr;

        public override void OnInit(SerializedProperty property, GUIContent label)
        {
            if (attribute is DisplayObjectFieldsAttribute tAttr)
            {
                attr = tAttr;
                property.isExpanded = tAttr.defaultExpanded;
            }
        }

        public override bool OnGUILayout(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference) // 若目標不是 UnityObject 則不繪製
            {
                return false;
            }
            using (var propScope = new EditorGUI.PropertyScope(position, label, property))
            {
                BetterGUILayout.PropertyField(property, label);
                if (IsGUI)
                {
                    property.isExpanded = BetterGUILayout.Foldout(property.isExpanded, "", true);
                }
                if (property.isExpanded)
                {
                    BetterGUILayout.SeparatorLine(2, 4);
                    using (new EditorGUI.DisabledScope(attr.readOnlyFields))
                    {
                        var data = property.objectReferenceValue;
                        if (data is null)
                        {
                            return true;
                        }
                        var dataSO = new SerializedObject(data);
                        var dataSP = dataSO.GetIterator();
                        using (new EditorGUI.IndentLevelScope(1))
                        {
                            EditorGUI.indentLevel++;
                            dataSP.NextVisible(true);
                            if (!attr.skipScriptField)
                            {
                                using (new EditorGUI.DisabledScope(true))
                                {
                                    BetterGUILayout.PropertyField(dataSP);
                                }
                            }
                            using (var check = new EditorGUI.ChangeCheckScope())
                            {

                                while (dataSP.NextVisible(false))
                                {
                                    if (!attr.skipFieldNames.Contains(dataSP.name))
                                    {
                                        BetterGUILayout.PropertyField(dataSP);
                                    }
                                }
                                if (check.changed)
                                {
                                    dataSP.serializedObject.ApplyModifiedProperties();
                                }
                            }
                        }
                    }
                }
                return true;
            }
        }
    }
}
