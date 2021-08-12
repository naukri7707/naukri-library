using Naukri.BetterAttribute;
using NaukriEditor.BetterInspector.Core;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterInspector
{
    [CustomInspectorDrawer(typeof(ReadOnlyAttribute), true)]
    public class ReadOnlyDrawer : InspectorPropertyDrawer
    {
        private EditorGUI.DisabledScope disableScope;

        public override void OnBeforeGUILayout(GUIContent label)
        {
            disableScope = new EditorGUI.DisabledScope(true);
        }

        public override void OnAfterGUILayout(GUIContent label)
        {
            disableScope.Dispose();
        }
    }
}
