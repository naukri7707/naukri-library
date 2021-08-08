using Naukri.Unity.BetterAttribute;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(PropertyUsageAttribute))]
    public class PropertyUsageAttributeDrawer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            var attr = attribute as PropertyUsageAttribute;
            LayoutContainer(
              () => property.objectReferenceValue = EditorGUI.ObjectField(
                  position,
                  property.displayName,
                  property.objectReferenceValue,
                  attr.type,
                  attr.allowSceneObject),
              EditorGUI.GetPropertyHeight(property)
              );
        }
    }
}