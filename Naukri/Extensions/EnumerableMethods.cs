using System;
using System.Collections.Generic;

namespace Naukri.Extensions
{
    public static class EnumerableMethods
    {
        public static IEnumerable<(T value, int index)> WithIndex<T>(this IEnumerable<T> self)
        {
            int idx = 0;
            foreach (var item in self)
            {
                yield return (item, idx++);
            }
        }

        public static TResult[] CreateSnapshots<T, TResult>(this IEnumerable<T> self, Func<T, TResult> target)
        {
            List<TResult> snapshots = null;
            self.CreateSnapshots(target, snapshots);
            return snapshots.ToArray();
        }

        public static IEnumerable<T> CreateSnapshots<T, TResult>(this IEnumerable<T> self, Func<T, TResult> target, List<TResult> snapshots)
        {
            snapshots = new List<TResult>();
            foreach (var item in self)
            {
                snapshots.Add(target(item));
                yield return item;
            }
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self)
            {
                action?.Invoke(item);
                yield return item;
            }
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T, int> action)
        {
            int idx = 0;
            foreach (var item in self)
            {
                action(item, idx++);
                yield return item;
            }
        }
    }
}