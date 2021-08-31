using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterAttribute
{

    [CustomPropertyDrawer(typeof(CustomObjectFieldAttribute))]
    public class CustomObjectFieldDrawer : BetterPropertyDrawer
    {
        public override IEnumerable<BetterGUIWrapper> OnGUILayout(SerializedProperty property, GUIContent label, bool isOnGUI)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                var attr = attribute as CustomObjectFieldAttribute;
                yield return BetterGUILayout.Wrapper(() =>
                {
                    property.objectReferenceValue = EditorGUI.ObjectField(
                        position,
                        property.displayName,
                        property.objectReferenceValue,
                        attr.type,
                        attr.allowSceneObject
                        );
                });
            }
        }
    }
}