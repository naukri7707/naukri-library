using System.Linq;
using System.Text;
using Naukri.Extensions;
using UnityEditor;
using UnityEngine;

namespace Naukri
{
    public static class EditorUnityPath
    {
        public static void CreateDirectory(string path)
        {
            const string Assets = "Assets";
            var dirs = UnityPath.GetDirectories(path);
            var builder = new StringBuilder(Assets);
            var currentDir = Assets;
            for (var i = dirs[0] is Assets ? 1 : 0; i < dirs.Length; i++)
            {
                var parentFolder = currentDir;
                currentDir = builder.Append('/').Append(dirs[i]).ToString();
                if (!UnityEditor.AssetDatabase.IsValidFolder(currentDir))
                {
                    UnityEditor.AssetDatabase.CreateFolder(parentFolder, dirs[i]);
                }
            }
        }

        public static bool AssetExist(string path)
        {
            return AssetDatabase.GetMainAssetTypeAtPath(path) != null;
        }
    }
}