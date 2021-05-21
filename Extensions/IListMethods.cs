using System;
using System.Collections;
using System.Collections.Generic;

namespace Naukri.Extensions
{
    public static class IListMethods
    {
        public static bool IndexExist(this Array self, int index)
        {
            return (index >= 0) & (index < self.Length);
        }
        
        public static bool HasElement(this IList self)
        {
            return self.Count > 0;
        }

        public static void CompareTo<T>(this List<T> self, IEnumerable<T> other, out List<T> keep, out List<T> add, out List<T> remove)
        {
            keep = new List<T>();
            add = new List<T>();
            remove = new List<T>();
            var keepFlag = new bool[self.Count];
            //
            foreach (var element in other)
            {
                var idx = self.IndexOf(element);
                if (idx is -1)
                {
                    add.Add(element);
                }
                else
                {
                    keep.Add(element);
                    keepFlag[idx] = true;
                }
            }
            //
            for (var i = 0; i < keepFlag.Length; i++)
            {
                if (keepFlag[i] is false)
                {
                    remove.Add(self[i]);
                }
            }
        }
    }
}