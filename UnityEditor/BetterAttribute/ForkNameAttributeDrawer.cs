using System.Reflection;
using Naukri.Unity.BetterAttribute;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Naukri.UnityEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(ForkNameAttribute), true)]
    public class ForkNameAttributeDrawer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            var attr = attribute as ForkNameAttribute;
            Assert.IsNotNull(attr);
            BetterGUILayout.PropertyField(
                property,
                new GUIContent(
                    CheckValue(property)
                        ? attr.trueForkName
                        : attr.falseForkName
                )
            );
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