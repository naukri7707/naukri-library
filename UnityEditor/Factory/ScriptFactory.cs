using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace Naukri.UnityEditor.Factory
{
    /// <summary>
    /// 使用 ScriptTemplate 建立腳本 <para/>
    /// <para/> 使用範例 : 
    /// <example>
    /// <code>
    /// <para/>[MenuItem("Assets/Create/MyFolder/MyScript", false, 89)]
    /// <para/>private static void CreateMyScript()
    /// <para/>{
    /// <para/>    ScriptTemplate.Replace("#MYTAG", "replaceText");
    /// <para/>    ScriptTemplate.Create("MyScriptTemplate.cs", "NewMyScript.cs");
    /// <para/>}
    /// <para/>
    /// <para/>public static void Replace(string srcText, string dstText)
    /// <para/>{
    /// <para/>    replaceList.Add((srcText, dstText));
    /// <para/>}
    /// </code>
    /// </example>
    /// </summary>
    public static class ScriptFactory
    {

        private static readonly List<(string src, string dst)> replaceList = new List<(string src, string dst)>();

        private static readonly Texture2D scriptIcon = (EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D);

        /// <summary>
        /// Use this method if your template named style is like "MyScript.cs.txt" 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Create<T>()
        {
            var name = typeof(T).Name;
            Create($"{name}.cs", $"New{name}.cs");
        }

        public static void Create(string templateName, string defaultScriptName)
        {
            string[] guids = AssetDatabase.FindAssets(templateName);
            if (guids.Length == 0)
            {
                Debug.LogWarning($"{templateName} not found in asset database");
                return;
            }
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            CreateFromTemplate(defaultScriptName, path);
        }

        public static void Replace(string tag, string replaceText)
        {
            replaceList.Add((tag, replaceText));
        }

        private static void CreateFromTemplate(string initialName, string templatePath)
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<DoCreateCodeFile>(),
                initialName,
                scriptIcon,
                templatePath
                );
        }

        private class DoCreateCodeFile : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                var o = CreateScript(pathName, resourceFile);
                ProjectWindowUtil.ShowCreatedAsset(o);
            }
        }

        private static Object CreateScript(string pathName, string templatePath)
        {
            var className = Path.GetFileNameWithoutExtension(pathName).Replace(" ", string.Empty);

            var encoding = new UTF8Encoding(true, false);

            if (File.Exists(templatePath))
            {
                StreamReader reader = new StreamReader(templatePath);
                var templateText = reader.ReadToEnd();
                reader.Close();

                templateText = templateText.Replace("#SCRIPTNAME#", className);
                templateText = templateText.Replace("#NOTRIM#", string.Empty);
                foreach (var (src, dst) in replaceList)
                {
                    templateText = templateText.Replace(src, dst);
                }
                replaceList.Clear();

                StreamWriter writer = new StreamWriter(Path.GetFullPath(pathName), false, encoding);
                writer.Write(templateText);
                writer.Close();

                AssetDatabase.ImportAsset(pathName);
                return AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
            }
            else
            {
                Debug.LogError($"The template file was not found: {templatePath}");
                return null;
            }
        }
    }
}
