using System;
using System.Collections.Generic;

namespace Naukri.Collections.Generic
{
    public class MaxHeap<T> : Heap<T> where T : IComparable<T>, IEquatable<T>
    {
        public T Max
        {
            get => Peak;
            set => Peak = value;
        }

        public T Min
        {
            get => Valley;
            set => Valley = value;
        }

        public MaxHeap() : base() { }

        public MaxHeap(int capacity) : base(capacity) { }
        
        public MaxHeap(IEnumerable<T> collection) : base(collection) { }

        public T[] GetAllMaxValues() => GetAllPeakValues();

        public T[] GetAllMinValues() => GetAllValleyValues();

        protected override int OnCompare(T lhs, T rhs)
        {
            return lhs.CompareTo(rhs);
        }
    }
}