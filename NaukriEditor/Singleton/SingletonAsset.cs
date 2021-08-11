using System.Reflection;
using Naukri;
using Naukri.Singleton;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.Singleton
{
    public abstract class SingletonAsset<T> : SingletonScriptableObject where T : SingletonAsset<T>
    {
        private static readonly object loadLock = new object();

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance is null)
                {
                    lock (loadLock)
                    {
                        if (TryLoadAsset(out instance))
                        {
                            instance.OnSingletonLoaded();
                        }
                        else
                        {
                            throw new UnityException($"Create asset failed at \"{typeof(T).Name}\"");
                        }
                    }
                }
                return instance;
            }
        }

        private static AssetPathAttribute Path
        {
            get
            {
                var type = typeof(T);
                var res = type.GetCustomAttribute<AssetPathAttribute>();
                if (res is null)
                {
                    throw new UnityException($"You have to define \"{nameof(AssetPathAttribute)}\" first at \"{type.Name}\"");
                }
                return res;
            }
        }

        private static bool TryLoadAsset(out T asset)
        {
            asset = AssetDatabase.LoadAssetAtPath<T>(Path.assetPath);
            return asset != null;
        }

        protected virtual void OnSingletonLoaded() { }
    }
}