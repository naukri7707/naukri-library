using Naukri.Unity.BetterAttribute;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(CustomObjectFieldAttribute))]
    public class CustomObjectFieldDrawer : BetterPropertyDrawer
    {
        public override bool OnGUILayout(SerializedProperty property, GUIContent label)
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
    }
}