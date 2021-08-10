using System.Reflection;
using Naukri.Unity.BetterAttribute;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Naukri.UnityEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(DisplayWhenFieldEqualAttribute), true)]
    public class DisplayWhenFieldEqualDrawer : BetterPropertyDrawer
    {
        public override bool OnGUILayout(SerializedProperty property, GUIContent label)
        {
            return !CheckValue(property);
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
            Assert.IsNotNull(attr);
            var target = property.serializedObject.targetObject;
            var type = target.GetType();
            var value = type.GetField(attr.fieldName ?? "",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(target);
            return value?.Equals(attr.value) ^ isNot ?? false;
        }
    }
}