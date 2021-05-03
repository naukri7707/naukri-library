using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;
using UnityEngine;

namespace Naukri.Unity.Serializable
{
    [Serializable]
    public class SerializableHashSet<T> : ISerializationCallbackReceiver, IReadOnlyCollection<T>, ISet<T>, IDeserializationCallback, ISerializable
    {
        private readonly HashSet<T> self;
        
        [SerializeField]
        private List<T> values = new List<T>();

        [SerializeField]
        private T newData;

        public SerializableHashSet() { self = new HashSet<T>(); }

        public SerializableHashSet(IEnumerable<T> collection) { self = new HashSet<T>(collection); }
        public SerializableHashSet(IEqualityComparer<T> comparer) { self = new HashSet<T>(comparer); }
        public SerializableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) { self = new HashSet<T>(collection, comparer); }

        public void OnBeforeSerialize()
        {
            values.Clear();
            values = new List<T>(self);
        }

        public void OnAfterDeserialize()
        {
            var isAddValue = values.Count > self.Count;
            self.Clear();
            if (isAddValue)
            {
                values[values.Count - 1] = newData;
            }
            foreach (var item in values)
            {
                if (item != null)
                {
                    Add(item);
                }
            }
        }

        public int Count => self.Count;
        public IEqualityComparer<T> Comparer => self.Comparer;

        public bool IsReadOnly => throw new NotImplementedException();

        public static IEqualityComparer<HashSet<T>> CreateSetComparer() => HashSet<T>.CreateSetComparer();
        public bool Add(T item) => self.Add(item);
        public void Clear() => self.Clear();
        public bool Contains(T item) => self.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => self.CopyTo(array, arrayIndex);
        public void CopyTo(T[] array, int arrayIndex, int count) => self.CopyTo(array, arrayIndex, count);
        public void CopyTo(T[] array) => self.CopyTo(array);
        public void ExceptWith(IEnumerable<T> other) => self.ExceptWith(other);

        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => self.GetObjectData(info, context);
        public void IntersectWith(IEnumerable<T> other) => self.IntersectWith(other);
        public bool IsProperSubsetOf(IEnumerable<T> other) => self.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<T> other) => self.IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<T> other) => self.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<T> other) => self.IsSupersetOf(other);
        public virtual void OnDeserialization(object sender) => self.OnDeserialization(sender);
        public bool Overlaps(IEnumerable<T> other) => self.Overlaps(other);
        public bool Remove(T item) => self.Remove(item);
        public int RemoveWhere(Predicate<T> match) => self.RemoveWhere(match);
        public bool SetEquals(IEnumerable<T> other) => self.SetEquals(other);
        public void SymmetricExceptWith(IEnumerable<T> other) => self.SymmetricExceptWith(other);
        public void TrimExcess() => self.TrimExcess();
        public void UnionWith(IEnumerable<T> other) => self.UnionWith(other);

        IEnumerator IEnumerable.GetEnumerator() => self.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => self.GetEnumerator();

        void ICollection<T>.Add(T item) => self.Add(item);
    }
}