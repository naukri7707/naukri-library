using Naukri;
using Naukri.Event;
using NaukriEditor.Factory;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NaukriEditor
{
    [CustomEditor(typeof(EventInvoker), true)]
    public class EventInvokerDrawer : Editor
    {
        public EventInvoker self;

        public ReorderableList reorderableList;

        public GUIStyle focusedBackgroundStyle;

        public GUIStyle selectedBackgroundStyle;

        private void OnEnable()
        {
            self = target as EventInvoker;
            focusedBackgroundStyle = new GUIStyle
            {
                normal = { background = TextureFactory.SolidColor(new Color32(44, 93, 135, 255)) }
            };
            selectedBackgroundStyle = new GUIStyle
            {
                normal = { background = TextureFactory.SolidColor(new Color32(77, 77, 77, 255)) }
            };
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {
            if (reorderableList is null)
            {
                var callersSP = serializedObject.FindProperty(nameof(EventInvoker.Callers));
                reorderableList = new ReorderableList(serializedObject, callersSP)
                {
                    drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Invoke List"); },
                    drawElementCallback = (rect, index, selected, focused) =>
                    {
                        var callerSP = callersSP.GetArrayElementAtIndex(index);
                        EditorGUI.indentLevel++;
                        var buttonRect = new Rect
                        {
                            xMin = rect.xMax - 80,
                            xMax = rect.xMax,
                            yMin = rect.yMin,
                            yMax = rect.yMin + 18
                        };
                        if (GUI.Button(buttonRect, "Invoke"))
                        {
                            self.Callers[index].TargetMethod.Invoke();
                            reorderableList.index = index;
                        }

                        var hotKey = self.Callers[index].hotKey;
                        var hotKeyString = hotKey == KeyCode.None ? "" : $" ({hotKey})";
                        EditorGUI.PropertyField(rect, callerSP, new GUIContent($"{callerSP.displayName}{hotKeyString}"), true);
                        EditorGUI.indentLevel--;
                        reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
                    },
                    elementHeightCallback = index =>
                    {
                        var property = callersSP.GetArrayElementAtIndex(index);
                        return EditorGUI.GetPropertyHeight(property, new GUIContent(property.displayName));
                    },
                    onAddCallback = list =>
                    {
                        callersSP.InsertArrayElementAtIndex(callersSP.arraySize);
                        callersSP.serializedObject.ApplyModifiedProperties();
                    },
                    onRemoveCallback = list =>
                    {
                        callersSP.DeleteArrayElementAtIndex(reorderableList.index);
                        callersSP.serializedObject.ApplyModifiedProperties();
                    },
                    drawElementBackgroundCallback = (rect, index, selected, focused) =>
                    {
                        if (focused)
                        {
                            GUI.Box(rect, "", focusedBackgroundStyle);
                        }
                        else if (selected)
                        {
                            GUI.Box(rect, "", selectedBackgroundStyle);
                        }
                    },
                };
            }
            //
            var worksOnRuntimeSP = serializedObject.FindProperty(nameof(EventInvoker.runtimeHotKey));
            EditorGUILayout.PropertyField(worksOnRuntimeSP);
            EditorGUILayout.Space(2);
            reorderableList.DoLayoutList();
        }
    }
}