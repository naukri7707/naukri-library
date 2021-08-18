using Naukri.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Naukri.Singleton
{
    public abstract class SingletonBehaviour<T> : NaukriBehaviour where T : SingletonBehaviour<T>
    {
        protected virtual bool DestroyOnLoad => false;

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
                        // 檢查 Hierarchy 是否有包含有該類的物件
                        instance = FindObjectOfType<T>();
                        if (instance != null || TryCreateInstance(out instance))
                        {
                            instance.OnSingletonLoaded();
                        }
                    }
                }

                return instance;
            }
        }

        private static bool TryCreateInstance(out T component)
        {
            var inst = new GameObject
            {
                name = $"{typeof(T).Name} (Singleton)",
                hideFlags = HideFlags.HideAndDontSave, // 隱藏該物件
            };
            // 不在場景轉換時刪除該物件
            component = inst.AddComponent<T>();
            if (!component.DestroyOnLoad)
            {
                DontDestroyOnLoad(inst);
            }
            return component != null;
        }

        protected virtual void OnSingletonLoaded() { }
    }
}