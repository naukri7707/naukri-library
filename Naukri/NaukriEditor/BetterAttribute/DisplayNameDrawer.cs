using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(DisplayNameAttribute))]
    public class DisplayNameDrawer : BetterPropertyDrawer
    {
        public override void OnBeforeGUILayout(SerializedProperty property, GUIContent label)
        {
            var attr = attribute as DisplayNameAttribute;
            label.text = attr.name;
        }
    }
}