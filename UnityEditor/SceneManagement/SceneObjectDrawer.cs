using Naukri.Unity.SceneManagement;
using Naukri.UnityEditor.BetterAttribute.Core;
using UnityEditor;
using UnityEngine;

namespace Naukri.UnityEditor.SceneManagement
{
    [CustomPropertyDrawer(typeof(SceneObject))]
    public class SceneObjectDrawer : BetterPropertyDrawer
    {
        public override void OnInit(SerializedProperty property, GUIContent label)
        {
            var sceneAssetSP = property.FindPropertyRelative("_sceneAsset");
            var sceneNameSP = property.FindPropertyRelative("_sceneName");
            if ((sceneAssetSP.objectReferenceValue is SceneAsset sceneAsset) && sceneAsset.name != sceneNameSP.stringValue) // 在場景名稱與序列化名稱不同時，重置本欄位
            {
                sceneAssetSP.objectReferenceValue = null;
                sceneNameSP.stringValue = ""; // 不做自動修正以強調本欄位對場景重新命名的不可追蹤性
                sceneAssetSP.serializedObject.ApplyModifiedProperties();
            }
        }

        public override void OnGUILayout(SerializedProperty property, GUIContent label)
        {
            var sceneAssetSP = property.FindPropertyRelative("_sceneAsset");
            var sceneNameSP = property.FindPropertyRelative("_sceneName");
            BetterGUILayout.BeginChangeCheck();
            Object newValue = default;
            LayoutContainer(
                ref newValue,
                () => EditorGUI.ObjectField(
                    position,
                    new GUIContent(property.displayName),
                    sceneAssetSP.objectReferenceValue,
                    typeof(SceneAsset), false),
                EditorGUI.GetPropertyHeight(property)
                );

            if (BetterGUILayout.EndChangeCheck())
            {
                sceneAssetSP.objectReferenceValue = newValue;
                sceneNameSP.stringValue = (sceneAssetSP.objectReferenceValue is SceneAsset sceneAsset) ? sceneAsset.name : "";
            }
        }
    }
}