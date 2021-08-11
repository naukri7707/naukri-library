using System.Reflection;
using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(ForkNameAttribute), true)]
    public class ForkNameDrawer : BetterPropertyDrawer
    {
        public override void OnBeforeGUILayout(SerializedProperty property, GUIContent label)
        {
            var attr = attribute as ForkNameAttribute;
            label.text = CheckValue(property)
                ? attr.trueForkName
                : attr.falseForkName;
        }

        private bool CheckValue(SerializedProperty property)
        {
            var attr = attribute as ForkNameAttribute;
            Assert.IsNotNull(attr);
            var target = property.serializedObject.targetObject;
            var type = target.GetType();
            var value = type
                .GetField(attr.fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(target);
            return value != null && value.Equals(attr.value);
        }
    }
}