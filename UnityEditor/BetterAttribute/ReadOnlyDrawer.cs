using Naukri.Unity.BetterAttribute;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : BetterPropertyDrawer
    {
        EditorGUI.DisabledScope disableScope;

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