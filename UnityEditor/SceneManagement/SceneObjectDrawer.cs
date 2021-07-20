using Naukri.Unity.SceneManagement;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.SceneManagement
{
    [CustomPropertyDrawer(typeof(SceneObject))]
    public class SceneObjectDrawer : BetterPropertyDrawer
    {
        SerializedProperty sceneAsset;
        SerializedProperty sceneName;

        public override void OnInit(SerializedProperty property, GUIContent label)
        {
            sceneAsset = property.FindPropertyRelative("_sceneAsset");
            sceneName = property.FindPropertyRelative("_sceneName");
            if((sceneAsset.objectReferenceValue as SceneAsset)?.name != sceneName.stringValue) // 在場景名稱與序列化名稱不同時，重置本欄位
            {
                sceneAsset.objectReferenceValue = null;
                sceneName.stringValue = ""; // 不做自動修正以強調本欄位對場景重新命名的不可追蹤性
                sceneAsset.serializedObject.ApplyModifiedProperties();
            }
        }

        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
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