using Naukri;
using Naukri.Extensions;
using Naukri.Singleton;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.Singleton
{
    public static class SingletonScriptableObjectCreator
    {
        [InitializeOnLoadMethod, MenuItem("Naukri/Create Singleton Asset")]
        public static void CreateAssetOnLoad()
        {
            var derivedTypes = TypeCache.GetTypesDerivedFrom<SingletonScriptableObject>();
            foreach (var type in derivedTypes)
            {
                if (type.IsAbstract) continue; // 略過抽象類別檢查及生成
                var attr = type.GetCustomAttribute<AssetPathAttribute>();
                if (attr is null)
                {
                    throw new UnityException($"{type.Name} 需要定義 {nameof(AssetPathAttribute)} 來指定 asset 路徑");
                }
                if (type.IsSubclassOfRawGeneric(typeof(SingletonResource<>)))
                {
                    _ = attr.ResourcePath; // 觸發裡面的 ResourcePath 檢查器
                }
                var path = attr.assetPath;
                if (!EditorUnityPath.AssetExist(path))
                {
                    EditorUnityPath.CreateDirectory(path);
                    var asset = ScriptableObject.CreateInstance(type);
                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.SaveAssets();
                }
            }
        }

    }
}
