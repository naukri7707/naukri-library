using UnityEngine;
using System.Reflection;
using UnityEditor;

namespace NaukriEditor
{
    [CustomPropertyDrawer(typeof(DisplayWhenFieldEqualAttribute), true)]
    public class DisplayWhenFieldEqualAttributeDarwer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            if(CheckValue(property))
            {
                BetterGUILayout.PropertyField(property);
            }
        }

        private bool CheckValue(SerializedProperty property)
        {
            var isNot = true;
            DisplayWhenFieldEqualAttribute attr = attribute as DisplayWhenFieldNotEqualAttribute;
            if (attr is null)
            {
                isNot = false;
                attr = attribute as DisplayWhenFieldEqualAttribute;
            }
            var target = property.serializedObject.targetObject;
            var type = target.GetType();
            var value = type.GetField(attr.fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(target);
            return value is null
                ? false
                : value.Equals(attr.value) ^ isNot;
        }
    }
}