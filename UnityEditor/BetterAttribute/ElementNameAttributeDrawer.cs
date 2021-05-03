using Naukri.Unity.BetterAttribute;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Naukri.UnityEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(ElementNameAttribute))]
    public class ElementNameAttributeDrawer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            var name = property.displayName.Split(' ');

            if (name[0] is "Element")
            {
                var attr = attribute as ElementNameAttribute;
                Assert.IsNotNull(attr);
                BetterGUILayout.PropertyField(property, new GUIContent($"{attr.name ?? "Element"} {name[1]}"), true);
            }
            else
            {
                BetterGUILayout.PropertyField(property, new GUIContent(property.displayName), true);
            }
        }
    }
}