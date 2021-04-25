using UnityEngine;
using UnityEditor;

namespace NaukriEditor
{
    [CustomPropertyDrawer(typeof(DisplayNameAttribute))]
    public class DisplayNameAttributeDarwer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            var attr = attribute as DisplayNameAttribute;
            BetterGUILayout.PropertyField(property, new GUIContent($"{attr.name}"), true);
        }
    }
}