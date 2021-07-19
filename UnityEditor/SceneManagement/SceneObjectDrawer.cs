using Naukri.Unity.SceneManagement;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.SceneManagement
{
    [CustomPropertyDrawer(typeof(SceneObject))]
    public class SceneObjectDrawer : BetterPropertyDrawer
    {
        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            SerializedProperty sceneAsset = property.FindPropertyRelative("sceneAsset");
            SerializedProperty sceneName = property.FindPropertyRelative("sceneName");

            BetterGUILayout.BeginChangeCheck();
            Object newValue = default;
            LayoutContainer(
                ref newValue,
                () => EditorGUI.ObjectField(
                    position,
                    new GUIContent(property.displayName),
                    sceneAsset.objectReferenceValue,
                    typeof(SceneAsset), false),
                EditorGUI.GetPropertyHeight(property)
                );

            if (BetterGUILayout.EndChangeCheck())
            {
                sceneAsset.objectReferenceValue = newValue;
                sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset)?.name ?? "";
            }
        }
    }
}