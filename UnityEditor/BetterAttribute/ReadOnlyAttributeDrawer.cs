using Naukri.Unity.BetterAttribute;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var prevGUIState = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = prevGUIState;
        }
    }
}