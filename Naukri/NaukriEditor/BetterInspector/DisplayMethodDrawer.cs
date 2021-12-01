using Naukri.BetterInspector;
using Naukri.Extensions;
using NaukriEditor.BetterInspector.Core;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterInspector
{
    [CustomInspectorDrawer(typeof(DisplayMethodAttribute), true)]
    public class DisplayMethodDrawer : InspectorMethodDrawer
    {
        private bool foldout;

        private ParameterInfo[] parameterInfos;

        private object[] parameterValues;

        public override void OnInit()
        {
            parameterInfos = MethodInfo.GetParameters().ToArray();
            parameterValues = parameterInfos.Select(it =>
                {
                    if (it.ParameterType.IsSubclassOf(typeof(UnityEngine.Object)))
                    {
                        return null;
                    }
                    else if (it.ParameterType == typeof(string))
                    {
                        return "";
                    }
                    return Activator.CreateInstance(it.ParameterType);
                }).ToArray();
        }

        public override bool OnGUILayout(GUIContent label)
        {
            if (parameterInfos.Length is 0)
            {
                using (var horizontal = new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(label);
                    if (GUILayout.Button("Invoke", GUILayout.Width(80)))
                    {
                        MethodInfo.Invoke(Target, parameterValues);
                    }
                }
            }
            else
            {
                using (var horizontal = new EditorGUILayout.HorizontalScope())
                {
                    foldout = EditorGUILayout.Foldout(foldout, label, true);
                    if (GUILayout.Button("Invoke", GUILayout.Width(80)))
                    {
                        MethodInfo.Invoke(Target, parameterValues);
                    }
                }
                if (foldout)
                {
                    using (new BetterGUILayout.IndentScope(2))
                    {
                        for (int i = 0; i < parameterInfos.Length; i++)
                        {
                            var parmLabel = new GUIContent(BetterGUILayout.GetDispalyName(parameterInfos[i].Name));
                            if (BetterGUILayout.AutoField(parameterInfos[i].ParameterType, parmLabel, parameterValues[i], out object res))
                            {
                                parameterValues[i] = res;
                            }
                            else
                            {
                                EditorGUILayout.LabelField("Unsupport Field");
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
