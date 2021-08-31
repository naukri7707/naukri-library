using System.Collections.Generic;
using System.Linq;
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

        public override IEnumerable<BetterGUIWrapper> OnGUILayout(SerializedProperty property, GUIContent label, bool isOnGUI)
        {
            // 若目標不是 UnityObject 則不繪製
            if (property.propertyType != SerializedPropertyType.ObjectReference) yield break;
            var foldOutRect = new Rect(position)
            {
                width = EditorGUIUtility.labelWidth
            };
            if (isOnGUI) property.isExpanded = EditorGUI.Foldout(foldOutRect, property.isExpanded, "", true);
            
            yield return BetterGUILayout.PropertyField(property, label);
            if (property.isExpanded)
            {
                yield return BetterGUILayout.SeparatorLine(2, 4);
                using (new EditorGUI.DisabledScope(attr.readOnlyFields))
                {
                    var data = property.objectReferenceValue;
                    if (data is null) yield break;
                    //
                    var dataSO = new SerializedObject(data);
                    var dataSP = dataSO.GetIterator();
                    //
                    using (new EditorGUI.IndentLevelScope(1))
                    {
                        dataSP.NextVisible(true);
                        if (!attr.skipScriptField)
                        {
                            using (new EditorGUI.DisabledScope(true))
                            {
                                yield return BetterGUILayout.PropertyField(dataSP);
                            }
                        }
                        using (var check = new EditorGUI.ChangeCheckScope())
                        {

                            while (dataSP.NextVisible(false))
                            {
                                if (!attr.skipFieldNames.Contains(dataSP.name))
                                {
                                    yield return BetterGUILayout.PropertyField(dataSP);
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
        }
    }
}
