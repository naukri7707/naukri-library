using Naukri.Unity.BetterAttribute;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(ExpandElementAttribute))]
    public class ExpandElementAttributeDrawer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            var nextProperty = property.Copy();
            var hasNextProperty = nextProperty.Next(false);
            while (property.NextVisible(true))
            {
                if (hasNextProperty && property.propertyPath == nextProperty.propertyPath)
                {
                    break;
                }
                BetterGUILayout.PropertyField(property, label, true);
            }
        }
    }
}