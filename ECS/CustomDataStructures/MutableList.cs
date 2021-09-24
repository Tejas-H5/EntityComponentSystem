using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.CustomDataStructures
{
    /// <summary>
    /// <para>
    /// A List implementation except the indexer returns by reference, 
    /// so objects in this list may by directly modified.
    /// This data structure is mainly designed to be used by the ECS component lists to iterate over structs.
    /// The normal List is actually a little faster for iterating over reference types.
    /// </para>
    /// 
    /// <para>
    /// So if you have a struct <c>X</c>, and a <c>MutableList&lt;X&gt; list</c>, you can do
    /// <c>list[index].Variable = value;</c>
    /// </para>
    /// 
    /// <para>
    /// This is beneficial for performance when iterating over a list of structs, so that the entire struct does not 
    /// need to be copied, modified, and then copied back.
    /// </para>
    /// 
    /// <para>
    /// Most features of the System.Collections.Generic.List have been discarded except the usefull ones, as well
    /// as a Swap method, as I have been using that a lot as well.
    /// </para>
    /// </summary>
    public class MutableList<T> 
    {
        public const int GrowthFactor = 2;

        private T[] _backingArray;
        int _length = 0;
        public int MinimumBackingArrayLength;

#if DEBUG
        public int Reallocations = 0;
#endif

        /// <summary>
        /// The intial capacity will also define the minimum allowed capacity
        /// </summary>
        public MutableList(int initCapacity = 10)
        {
            _backingArray = new T[initCapacity];
            MinimumBackingArrayLength = initCapacity;
        }

        public ref T this[int i] {
            get {
#if DEBUG
                testIndex(i);
#endif
                return ref _backingArray[i];
            }
        }

        public T Get(int i)
        {
            testIndex(i);

            return _backingArray[i];
        }

        public int Count {
            get {
                return _length;
            }
        }

        private void testIndex(int i)
        {
            if (i > _length || i < 0)
                throw new IndexOutOfRangeException("" + i + " is outside the bounds of this array (" + _length + ")");
        }

        public void Set(int i, T value)
        {
            testIndex(i);

            _backingArray[i] = value;
        }

        public void Add(T value)
        {
            if (_length == _backingArray.Length)
            {
                increaseSize();
            }

            _backingArray[_length] = value;
            _length++;
        }

        public void Insert(int index, T value)
        {
            Add(value);
            Array.Copy(_backingArray, index, _backingArray, index + 1, _length - index - 1);

            _backingArray[index] = value;
        }

        private void increaseSize()
        {
            int currentSize = _backingArray.Length;
            if (currentSize == 0)
                currentSize = 1;
            resizeBackingArray(currentSize * GrowthFactor);
        }

        private void resizeBackingArray(int newSize)
        {
            if (newSize < MinimumBackingArrayLength)
                newSize = MinimumBackingArrayLength;

            if (_backingArray.Length == newSize)
                return;

            T[] newArray = new T[newSize];
            Array.Copy(_backingArray, 0, newArray, 0, _length);
            _backingArray = newArray;

#if DEBUG
            Reallocations++;
#endif
        }

        public void RemoveAt(int index)
        {
            testIndex(index);

            for (int i = index; i < _length - 1; i++)
            {
                _backingArray[i] = _backingArray[i + 1];
            }

            _length--;
        }


        public void Swap(int indexA, int indexB)
        {
            testIndex(indexA);
            testIndex(indexB);

            T tmp = _backingArray[indexA];
            _backingArray[indexA] = _backingArray[indexB];
            _backingArray[indexB] = tmp;
        }

        public void Clear()
        {
            _length = 0;
        }

        public void TrimToSize()
        {
            resizeBackingArray(_length);
        }

        public void Sort()
        {
            Array.Sort(_backingArray, 0, _length);
        }

        public void RemoveRange(int i, int count)
        {
#if DEBUG
            testIndex(i);
            testIndex(i+count);
#endif

            Array.Copy(_backingArray, i + count, _backingArray, i, _length - (i + count));
            _length -= count;
        }
    }
}
