using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : BetterPropertyDrawer
    {
        private EditorGUI.DisabledScope disableScope;

        public override void OnBeforeGUILayout(SerializedProperty property, GUIContent label)
        {
            disableScope = new EditorGUI.DisabledScope(true);
        }

        public override void OnAfterGUILayout(SerializedProperty property, GUIContent label)
        {
            disableScope.Dispose();
        }
    }
}