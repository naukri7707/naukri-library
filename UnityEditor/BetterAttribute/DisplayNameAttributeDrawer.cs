using Naukri.Unity.BetterAttribute;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(DisplayNameAttribute))]
    public class DisplayNameAttributeDrawer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            var attr = attribute as DisplayNameAttribute;
            BetterGUILayout.PropertyField(property, new GUIContent($"{attr?.name}"), true);
        }
    }
}