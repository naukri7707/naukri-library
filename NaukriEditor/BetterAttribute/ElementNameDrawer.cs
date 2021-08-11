using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(ElementNameAttribute))]
    public class ElementNameDrawer : BetterPropertyDrawer
    {
        public override void OnBeforeGUILayout(SerializedProperty property, GUIContent label)
        {
            var name = property.displayName.Split(' ');

            if (name[0] is "Element")
            {
                var attr = attribute as ElementNameAttribute;
                label.text = $"{attr.elementName ?? "Element"} {name[1]}";
            }
        }
    }
}