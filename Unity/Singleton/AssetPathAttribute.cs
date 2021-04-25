using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Naukri
{
    public class AssetPathAttribute : Attribute
    {
        public readonly string assetPath;

        public string ResourcePath
        {
            get
            {
                var builder = new StringBuilder();
                var dirs = UnityPath.GetDirectories(assetPath);
                var isResource = false;
                foreach (var dir in dirs)
                {
                    builder.Append(dir);
                    builder.Append('/');
                    if (dir is "Resources")
                    {
                        isResource = true;
                        builder.Clear();
                    }
                }
                if (isResource)
                {
                    builder.Append(UnityPath.GetFileNameWithoutExtension(assetPath));
                    return builder.ToString();
                }
                else
                {
                    throw new UnityException($"{nameof(ResourcePath)} 須包含\'Resources\'資料夾");
                }
            }
        }

        public AssetPathAttribute(string assetPath)
        {
            var builder = new StringBuilder();
            if (Naukri.UnityPath.GetDirectoryRoot(assetPath) != "Assets")
            {
                builder.Append("Assets/");
            }
            builder.Append(assetPath);
            if (!Naukri.UnityPath.HasExtension(assetPath))
            {
                builder.Append(".asset");
            }
            this.assetPath = builder.ToString();
        }
    }
}