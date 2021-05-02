using Naukri.Unity.BetterAttribute;
using UnityEngine;
using UnityEditor;

namespace NaukriEditor
{
    [CustomPropertyDrawer(typeof(ElementNameAttribute))]
    public class ElementNameAttributeDarwer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            var name = property.displayName.Split(' ');

            if (name[0] is "Element")
            {
                var attr = attribute as ElementNameAttribute;
                BetterGUILayout.PropertyField(property, new GUIContent($"{attr.name} {name[1]}"), true);
            }
            else
            {
                BetterGUILayout.PropertyField(property, new GUIContent(property.displayName), true);
            }
        }
    }
}
