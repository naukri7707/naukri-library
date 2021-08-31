using Naukri;
using Naukri.BetterAttribute;
using Naukri.BetterInspector;
using Naukri.Extensions;
using Naukri.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Naukri.Collections.Generic
{
    public abstract class Heap<T> : IEnumerable<T> where T : IComparable<T>, IEquatable<T>
    {
        private readonly List<T> heap;

        private int lastHeapIndex = 0;

        public int Count => lastHeapIndex;

        protected T Peak
        {
            get => this[0];
            set => this[0] = value;
        }

        protected T Valley
        {
            get => this[GetLastValleyIndex()];
            set => this[GetLastValleyIndex()] = value;
        }

        public T this[int index]
        {
            get
            {
                var heapIndex = index + 1;
                EnsureIndex(heapIndex);
                return heap[heapIndex];
            }
            set
            {
                var heapIndex = index + 1;
                EnsureIndex(index + 1);
                var oldValue = heap[heapIndex];
                heap[heapIndex] = value;
                var compare = OnCompare(value, oldValue);
                if (compare > 0)
                {
                    HeapifyUp(heapIndex);
                }
                else if (compare < 0)
                {
                    HeapifyDown(heapIndex);
                }
            }
        }

        public Heap()
        {
            heap = new List<T>();
            heap.Add(default); // 空欄位讓新增/刪除資料可以透過位元運算完成
        }

        public Heap(int capacity)
        {
            heap = new List<T>(capacity);
            heap.Add(default);
        }

        public Heap(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new System.ArgumentNullException(nameof(collection));
            }
            heap = new List<T>();
            heap.Add(default);
            foreach (T item in collection)
            {
                Add(item);
            }
        }

        public T Pop()
        {
            var res = Peak;
            RemoveAtHeap(1);
            return res;
        }

        public T Peek()
        {
            return Peak;
        }

        public void Add(T item)
        {
            if (++lastHeapIndex < heap.Count)
            {
                heap[lastHeapIndex] = item;
            }
            else
            {
                heap.Add(item);
            }
            HeapifyUp(lastHeapIndex);
        }

        public void Remove(T item)
        {
            var index = 0;
            while (++index <= lastHeapIndex)
            {
                var compare = heap[index].CompareTo(item);
                if (compare <= 0)
                {
                    if (compare == 0)
                    {
                        RemoveAtHeap(index);
                    }
                    break;
                }
            }
        }

        public void RemoveAll(T item)
        {
            var index = 0;
            while (++index <= lastHeapIndex)
            {
                var compare = heap[index].CompareTo(item);
                if (compare <= 0)
                {
                    if (compare == 0)
                    {
                        RemoveAtHeap(index);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public T[] GetSortedArray()
        {
            var count = lastHeapIndex;
            var res = new T[count];
            for (int i = 0; i < count; i++)
            {
                res[i] = Pop();
            }
            lastHeapIndex = count;
            for (int i = 0; i < count; i++)
            {
                heap[i + 1] = res[i];
            }
            return res;
        }

        public bool Exists(Predicate<T> match)
        {
            return FindIndex(match) != -1;
        }

        public bool TryFind(Predicate<T> match, out T value)
        {
            var idx = FindIndex(match);
            if (idx == -1)
            {
                value = default;
                return false;
            }
            else
            {
                value = heap[idx + 1];
                return true;
            }
        }

        public T Find(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (int i = 1; i <= lastHeapIndex; i++)
            {
                if (match(heap[i]))
                {
                    return heap[i];
                }
            }

            return default;
        }

        public List<T> FindAll(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            var list = new List<T>();
            for (int i = 1; i <= lastHeapIndex; i++)
            {
                if (match(heap[i]))
                {
                    list.Add(heap[i]);
                }
            }

            return list;
        }

        private int FindIndex(Predicate<T> match)
        {
            return FindIndex(0, lastHeapIndex, match);
        }

        private int FindIndex(int startIndex, Predicate<T> match)
        {
            return FindIndex(startIndex, lastHeapIndex - startIndex, match);
        }

        private int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            if ((uint)startIndex > (uint)lastHeapIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }
            if (count < 0 || startIndex > lastHeapIndex - count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            int end = startIndex + count;
            for (int i = startIndex; i < end; i++)
            {
                if (match(heap[i + 1]))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Clear()
        {
            heap.Clear();
            lastHeapIndex = 0;
        }

        public void ClearFreeElements()
        {
            heap.RemoveRange(lastHeapIndex + 1, heap.Count - lastHeapIndex);
        }

        protected T[] GetAllPeakValues()
        {
            var peak = Peak;
            return this.Where(it => it.Equals(peak)).ToArray();
        }

        protected T[] GetAllValleyValues()
        {
            var valley = Valley;
            return this.Where(it => it.Equals(valley)).ToArray();
        }

        private void EnsureIndex(int index)
        {
            if (index <= 0 || index > lastHeapIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        private void RemoveAtHeap(int index)
        {
            var last = lastHeapIndex;
            heap.Swap(index, last);
            lastHeapIndex--;
            HeapifyDown(1);
        }

        private void HeapifyUp(int index)
        {
            var curr = index;
            var next = curr >> 1;
            while (next > 0 && OnCompare(heap[curr], heap[next]) > 0)
            {
                heap.Swap(curr, next);
                curr = next;
                next >>= 1;
            }
        }

        private void HeapifyDown(int index)
        {
            var curr = index;
            var next = curr << 1;
            var last = lastHeapIndex;
            while (next <= last)
            {
                if (next < last && OnCompare(heap[next], heap[next + 1]) < 0) next++;
                if (OnCompare(heap[curr], heap[next]) < 0)
                {
                    heap.Swap(curr, next);
                }
                else
                {
                    break;
                }
                curr = next;
                next <<= 1;
            }
        }

        private int GetLastValleyIndex()
        {
            var end = lastHeapIndex;
            var start = (end >> 1) + 1;
            var index = end;
            for (int i = end - 1; i >= start; i--)
            {
                if (OnCompare(heap[i], heap[index]) < 0)
                {
                    index = i;
                }
            }
            return index - 1; // 轉成 heap index
        }

        protected abstract int OnCompare(T lhs, T rhs);

        public IEnumerator<T> GetEnumerator()
        {
            var count = lastHeapIndex;
            for (int i = 1; i <= lastHeapIndex; i++)
            {
                if (lastHeapIndex != count)
                {
                    throw new InvalidOperationException("Collection was modified; enumeration operation may not execute");
                }
                yield return heap[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}