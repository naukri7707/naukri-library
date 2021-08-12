using Naukri.BetterInspector;
using NaukriEditor.BetterInspector.Core;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterInspector
{
    [CustomInspectorDrawer(typeof(DisplayPropertyAttribute), true)]
    public class DisplayPropertyDrawer : InspectorPropertyDrawer
    {
        public override bool OnGUILayout(GUIContent label)
        {
            using (new EditorGUI.DisabledScope(!PropertyInfo.CanWrite))
            {
                if (BetterGUILayout.AutoField(PropertyInfo.PropertyType, label, Value, out object res))
                {
                    if (PropertyInfo.CanWrite)
                    {
                        Value = res;
                    }
                    return true;
                }
                return false;
            }
        }
    }
}
