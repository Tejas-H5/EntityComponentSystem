using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.CustomDataStructures
{
    public class MultiList<T> : List<T>
    {
        int _listCount;
        public int Length {
            get {
                return base.Count / _listCount;
            }
        }

        public MultiList(int listCount)
        {
            _listCount = listCount;
        }

        public MultiList(int listCount, IEnumerable<T> collection) : base(collection)
        {
            _listCount = listCount;
        }

        public MultiList(int listCount, int capacity) : base(capacity)
        {
            _listCount = listCount;
        }

        public T this[int index, int listNumber] {
            get {
                return base[index * _listCount + listNumber];
            }
        }

        public void Add(T[] items)
        {
            for(int i = 0; i < items.Length; i++)
            {
                base.Add(items[i]);
            }
        }

        public new void RemoveAt(int index)
        {
            base.RemoveRange(index * _listCount, _listCount);
        }

        public void Swap(int a, int b)
        {
            a *= _listCount;
            b *= _listCount;
            for(int i = 0; i < _listCount; i++)
            {
                T temp = base[a+i];
                base[a + i] = base[b + i];
                base[b + i] = temp;
            }
        }
    }
}
