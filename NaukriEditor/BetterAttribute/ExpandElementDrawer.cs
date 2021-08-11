using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(ExpandElementAttribute))]
    public class ExpandElementDrawer : BetterPropertyDrawer
    {
        public override bool OnGUILayout(SerializedProperty property, GUIContent label)
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

            return true;
        }
    }
}