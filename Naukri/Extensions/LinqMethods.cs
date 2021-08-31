using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naukri.Extensions
{
    public static class LinqMethods
    {
        public static int IndexOf<T>(this IEnumerable<T> self, T item)
        {
            return IndexOf((IEnumerable)self, item);
        }

        public static int IndexOf<T>(this IEnumerable self, T item)
        {
            var idx = 0;
            foreach (var it in self)
            {
                if (it.Equals(item))
                {
                    return idx;
                }
                idx++;
            }
            return -1;
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> self, TSource defaultValue)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (self is IList<TSource> list)
            {
                if (list.Count > 0)
                {
                    return list[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> enumerator = self.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        return enumerator.Current;
                    }
                }
            }

            return defaultValue;
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (var item in self)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return defaultValue;
        }

        public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> self, TSource defaultValue)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (self is IList<TSource> list)
            {
                int count = list.Count;
                if (count > 0)
                {
                    return list[count - 1];
                }
            }
            else
            {
                using (IEnumerator<TSource> enumerator = self.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        TSource current;
                        do
                        {
                            current = enumerator.Current;
                        }
                        while (enumerator.MoveNext());
                        return current;
                    }
                }
            }

            return defaultValue;
        }

        public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate, TSource defaultValue)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            TSource res = defaultValue;
            foreach (TSource item in self)
            {
                if (predicate(item))
                {
                    res = item;
                }
            }

            return res;
        }

    }
}
