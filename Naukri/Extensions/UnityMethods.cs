using System;
using System.Collections.Generic;
using UnityEngine;

namespace Naukri.Extensions
{
    public static class UnityMethods
    {
        public static T GetOrAddComponent<T>(this Component self) where T : Component
        {
            return GetOrAddComponent<T>(self.gameObject);
        }

        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            return self.TryGetComponent<T>(out var comp) ? comp : self.AddComponent<T>();
        }
    }
}
