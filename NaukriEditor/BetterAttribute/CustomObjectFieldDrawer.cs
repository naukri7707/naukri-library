using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(CustomObjectFieldAttribute))]
    public class CustomObjectFieldDrawer : BetterPropertyDrawer
    {
        public override bool OnGUILayout(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                var attr = attribute as CustomObjectFieldAttribute;
                LayoutWrapper(
                  () => property.objectReferenceValue = EditorGUI.ObjectField(
                      position,
                      property.displayName,
                      property.objectReferenceValue,
                      attr.type,
                      attr.allowSceneObject),
                  EditorGUI.GetPropertyHeight(property)
                  );
                return true;
            }
            return false;
        }
    }
}