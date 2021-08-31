using System.Collections.Generic;
using System.Reflection;
using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(DisplayWhenFieldEqualAttribute), true)]
    public class DisplayWhenFieldEqualDrawer : BetterPropertyDrawer
    {
        public override IEnumerable<BetterGUIWrapper> OnGUILayout(SerializedProperty property, GUIContent label, bool isOnGUI)
        {
            if (!CheckValue(property)) yield return BetterGUILayout.EmptyField(0);
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
            foreach (var compareValue in attr.values)
            {
                if (value.Equals(compareValue))
                {
                    return !isNot;
                }
            }
            return isNot;
        }
    }
}