using System.Linq;
using System.Text;
using Naukri.Extensions;
using UnityEngine;

namespace Naukri
{
    public static class UnityPath
    {
        public static string GetDirectoryRoot(string path)
        {
            var dirs = GetDirectories(path);
            return dirs?[0];
        }

        public static string[] GetDirectories(string path)
        {
            var dirs = path.Split('\\', '/');
            return dirs.Take(dirs.Length - 1).ToArray();
        }

        public static string GetFileName(string path)
        {
            var dirs = path.Split('\\', '/');
            return dirs.HasElement() ? dirs[dirs.Length - 1] : null;
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            var fileName = GetFileName(path);
            var idx = fileName.Length;
            while (--idx > 0)
            {
                if (fileName[idx] is '.')
                {
                    return fileName.Substring(0, idx);
                }
            }

            throw new UnityException("GetFileNameWithoutExtension Failed");
        }

        public static string GetExtension(string path)
        {
            var fileName = GetFileName(path);
            for (var i = fileName.Length - 2; i >= 0; i--)
            {
                if (fileName[i] is '.')
                {
                    i++;
                    return fileName.Substring(i, fileName.Length - i);
                }
            }

            throw new UnityException("GetExtension Failed");
        }

        public static bool HasExtension(string path)
        {
            var fileName = GetFileName(path);
            for (var i = fileName.Length - 2; i >= 0; i--)
            {
                if (fileName[i] is '.')
                {
                    return true;
                }
            }

            return false;
        }

        public static void CreateDirectoryInEditor(string path)
        {
#if UNITY_EDITOR
            const string Assets = "Assets";
            var dirs = GetDirectories(path);
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
#endif
        }
    }
}