using System.Reflection;
using UnityEngine;

namespace Naukri.Singleton
{
    public abstract class SingletonResource<T> : SingletonScriptableObject where T : SingletonResource<T>
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
            asset = Resources.Load<T>(Path.ResourcePath);
            return asset != null;
        }

        protected virtual void OnSingletonLoaded() { }
    }
}