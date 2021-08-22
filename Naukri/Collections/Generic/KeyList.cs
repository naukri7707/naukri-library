using Naukri.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Naukri.Collections.Generic
{
    public class KeyList<TKey, TValue>
    {
        private readonly List<TValue> values = new List<TValue>();

        private readonly Dictionary<TKey, int> indexes = new Dictionary<TKey, int>();

        public int Count => values.Count;

        public TValue this[int index]
        {
            get => values[index];
            set => values[index] = value;
        }

        public TValue this[TKey key]
        {
            get => values[indexes[key]];
            set
            {
                if (indexes.TryGetValue(key, out var index))
                {
                    values[index] = value;
                }
                else
                {
                    indexes[key] = values.Count;
                    values.Add(value);
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            indexes.Add(key, values.Count);
            values.Add(value);
        }

        public void Remove(TValue value)
        {
            var idx = values.IndexOf(value);
            RemoveAt(idx);
        }

        public void RemoveKey(TKey key)
        {
            var index = indexes[key];
            indexes.Remove(key);
            values.RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            if (values.IsIndexValid(index))
            {
                values.RemoveAt(index);
                var removeKey = indexes.First(it => it.Value == index).Key;
                indexes.Remove(removeKey);
            }
        }
    }
}
