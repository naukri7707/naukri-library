using System;
using System.Collections.Generic;

namespace Naukri.Collections.Generic
{
    public class MinHeap<T> : Heap<T> where T : IComparable<T>, IEquatable<T>
    {
        public T Max
        {
            get => Valley;
            set => Valley = value;
        }

        public T Min
        {
            get => Peak;
            set => Peak = value;
        }

        public MinHeap() : base() { }

        public MinHeap(int capacity) : base(capacity) { }

        public MinHeap(IEnumerable<T> collection) : base(collection) { }

        public T[] GetAllMaxValues() => GetAllValleyValues();

        public T[] GetAllMinValues() => GetAllPeakValues();

        protected override int OnCompare(T lhs, T rhs)
        {
            return rhs.CompareTo(lhs);
        }
    }
}