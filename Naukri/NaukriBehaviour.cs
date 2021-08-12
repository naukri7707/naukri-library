using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Naukri
{
    public abstract class NaukriBehaviour : MonoBehaviour
    {
        public T[] GetComponentsRecursive<T>(int depth = 1, bool includeInactive = false)
        {
            return GetComponentsRecursiveImp(typeof(T), depth, includeInactive).Cast<T>().ToArray();
        }

        public Component[] GetComponentsRecursive(Type componentType, int depth = 1, bool includeInactive = false)
        {
            return GetComponentsRecursiveImp(componentType, depth, includeInactive).ToArray();
        }

        private List<Component> GetComponentsRecursiveImp(Type componentType, int depth, bool includeInactive)
        {
            var res = new List<Component>();

            void DFS(int depthLevel)
            {
                if (depthLevel-- is 0)
                {
                    return;
                }
                foreach (Transform child in transform)
                {
                    if (child.gameObject.activeSelf || includeInactive)
                    {
                        if (child.TryGetComponent(componentType, out var component))
                        {
                            res.Add(component);
                        }
                    }
                }
            }
            DFS(depth);

            return res;
        }
    }
}