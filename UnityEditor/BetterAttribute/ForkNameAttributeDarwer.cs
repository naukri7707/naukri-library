using UnityEngine;
using System.Reflection;
using UnityEditor;

namespace NaukriEditor
{
    [CustomPropertyDrawer(typeof(ForkNameAttribute), true)]
    public class ForkNameAttributeDarwer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            var attr = attribute as ForkNameAttribute;
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
            var target = property.serializedObject.targetObject;
            var type = target.GetType();
            var value = type.GetField(attr.fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(target);
            return value != null && value.Equals(attr.value);
        }
    }
}