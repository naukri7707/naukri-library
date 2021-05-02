using Naukri.Extensions;
using Naukri.Toast;
using UnityEngine;

namespace Naukri
{
    public static class Toaster
    {
        private static bool hideInHierarchy = true;

        public static bool HideInHierarchy
        {
            get => hideInHierarchy;
            set
            {
                hideInHierarchy = value;
                if (value)
                {
                    var flag = Manager.gameObject.hideFlags.AddFlag(HideFlags.HideInHierarchy);
                    Manager.gameObject.hideFlags = flag;
                }
                else
                {
                    var flag = Manager.gameObject.hideFlags.RemoveFlag(HideFlags.HideInHierarchy);
                    Manager.gameObject.hideFlags = flag;
                }
            }
        }

        private static ToastManager _manager;

        public static ToastManager Manager
        {
            get
            {
                if (_manager is null)
                {
                    if (Resources.Load("DefaultToastManager") is GameObject prefab)
                    {
                        var inst = Object.Instantiate(prefab);
                        _manager = inst.GetComponent<ToastManager>();
                        if (_manager is null)
                        {
                            throw new UnityException($"Can not found {nameof(ToastManager)} in DefaultToastCanvas");
                        }
                        Object.DontDestroyOnLoad(inst);
                        HideInHierarchy = hideInHierarchy; // 觸發 setter
                    }
                    else
                    {
                        throw new UnityException("Can not found DefaultToastCanvas");
                    }
                }
                return _manager;
            }
        }

        public static void Create(string message)
        {
            Create(message, Color.white);
        }

        public static void Create(string message, Color color)
        {
            var msgInfo = new ToastMessage
            {
                text = message,
                color = color
            };
            Manager.Toast(msgInfo);
        }

        public static void ClearToast()
        {
            Manager.ClearToast();
        }

        public static void ClearMessageQueue()
        {
            Manager.ClearMessageQueue();
        }

        public static void ClearAll()
        {
            Manager.ClearAll();
        }
    }
}