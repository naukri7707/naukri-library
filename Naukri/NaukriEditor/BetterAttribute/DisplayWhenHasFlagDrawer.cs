using System.Collections.Generic;
using System.Reflection;
using Naukri;
using Naukri.BetterAttribute;
using Naukri.Extensions;
using NaukriEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(DisplayWhenHasFlagAttribute), true)]
    public class DisplayWhenHasFlagDrawer : BetterPropertyDrawer
    {
        public override IEnumerable<BetterGUIWrapper> OnGUILayout(SerializedProperty property, GUIContent label, bool isOnGUI)
        {
            if (!CheckValue(property)) yield return BetterGUILayout.EmptyField(0);
        }

        private bool CheckValue(SerializedProperty property)
        {
            var attr = attribute as DisplayWhenHasFlagAttribute;
            var target = property.serializedObject.targetObject;
            var value = fieldInfo.GetValue(target);
            return attr.flag.HasFlag((Flag)value);
        }
    }
}