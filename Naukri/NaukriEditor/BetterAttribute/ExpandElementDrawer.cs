using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(ExpandElementAttribute))]
    public class ExpandElementDrawer : BetterPropertyDrawer
    {
        public override IEnumerable<BetterGUIWrapper> OnGUILayout(SerializedProperty property, GUIContent label, bool isOnGUI)
        {
            var nextProperty = property.Copy();
            var hasNextProperty = nextProperty.Next(false);
            while (property.NextVisible(true))
            {
                if (hasNextProperty && property.propertyPath == nextProperty.propertyPath)
                {
                    break;
                }
                yield return BetterGUILayout.PropertyField(property, label, true);
            }
        }
    }
}