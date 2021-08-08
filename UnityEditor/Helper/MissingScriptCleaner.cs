using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking.Types;
using Codice.Client.Common;
using System.Reflection;

public class MissingScriptCleaner : EditorWindow
{
    [MenuItem("Naukri/Tools/Missing Script Cleaner")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(MissingScriptCleaner));
        window.titleContent = new GUIContent("Missing Script Cleaner");
    }

    public List<GameObject> objectsToClean;

    public List<GameObject> missingScriptObjects;

    private bool searched = false;

    private bool cleanSourcePrefab = true;

    public void OnGUI()
    {
        var editorWindowSO = new SerializedObject(this);
        var objectsToCleanSP = editorWindowSO.FindProperty(nameof(objectsToClean));
        var missingScriptObjectsSP = editorWindowSO.FindProperty(nameof(missingScriptObjects));
        EditorGUILayout.PropertyField(objectsToCleanSP);
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Add Selected GameObject"))
            {
                var selected = Selection.gameObjects.OrderBy(it => it.name);
                foreach (var item in selected)
                {
                    if (!objectsToClean.Contains(item))
                    {
                        objectsToClean.Add(item);
                    }
                }
            }
            if (GUILayout.Button("Clear"))
            {
                searched = false;
                objectsToClean.Clear();
            }
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Search Missing Script"))
        {
            missingScriptObjects = objectsToClean
                .SelectMany(it => it.GetComponentsInChildren<Transform>(true))
                .Where(it => GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(it.gameObject) > 0)
                .Select(it => it.gameObject)
                .Distinct()
                .ToList();
            searched = true;
        }
        if (searched)
        {
            EditorGUILayout.PropertyField(missingScriptObjectsSP);
            cleanSourcePrefab = EditorGUILayout.Toggle("Clean Source Prefab", cleanSourcePrefab);
            if (GUILayout.Button("Clean Missing Script"))
            {
                CleanMissingScript(cleanSourcePrefab);
            }
        }
    }

    private void CleanMissingScript(bool cleanSourcePrefab)
    {
        var missingScriptPrefabCount = 0;
        var missingScriptGameObjectCount = 0;
        var missingScriptCount = 0;
        var visitedPrefabs = new HashSet<Object>();

        void CleanDirtyPrefab(GameObject instance)
        {
            var sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(instance);
            if (sourcePrefab != null && visitedPrefabs.Add(sourcePrefab))
            {
                CleanDirtyPrefab(sourcePrefab);

                missingScriptPrefabCount++;
                missingScriptCount += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(sourcePrefab);
            }
        }

        foreach (var target in missingScriptObjects)
        {
            if (cleanSourcePrefab && PrefabUtility.IsPartOfAnyPrefab(target))
            {
                CleanDirtyPrefab(target);
            }
            var removeCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(target);
            missingScriptGameObjectCount += removeCount > 0 ? 1 : 0;
            missingScriptCount += removeCount;
        }

        EditorUtility.DisplayDialog(
            "完成",
            missingScriptCount == 0
                ? $"沒有找到 Missing Script"
                : cleanSourcePrefab
                ? $"共從 {missingScriptGameObjectCount} 個 {nameof(GameObject)} 和 {missingScriptPrefabCount} 個 SourcePrefab 中清除 {missingScriptCount} 個 Missing Script"
                : $"共從 {missingScriptGameObjectCount} 個 {nameof(GameObject)} 中清除 {missingScriptCount} 個 Missing Script",
            "確認"
            );
    }
}