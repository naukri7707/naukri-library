using Naukri.Unity.BetterAttribute;
using UnityEngine;
using UnityEditor;

namespace NaukriEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            var prevGUIState = GUI.enabled;
            GUI.enabled = false;
            BetterGUILayout.PropertyField(property, label);
            GUI.enabled = prevGUIState;
        }
    }
}