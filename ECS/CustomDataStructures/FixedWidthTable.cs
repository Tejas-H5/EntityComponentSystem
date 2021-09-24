using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.CustomDataStructures
{
    public class FixedWidthTable<T> where T : unmanaged
    {
        MutableList<T> _backingList = new MutableList<T>();
        int _width;

        public int Rows {
            get {
                return _backingList.Count/_width;
            }
        }
        public int Columns {
            get {
                return _width;
            }
        }

        public FixedWidthTable(int width)
        {
            if (width == 0)
                throw new Exception("table width MUST NOT BE 0");

            _width = width;
        }

        public T this[int row, int column] {
            get {
                return _backingList[(row * _width) + column];
            }
        }

        public void Add(T[] items)
        {
#if DEBUG
            if (items.Length != _listCount)
                throw new Exception("This array's length is not the same as the list count");
#endif

            for(int i = 0; i < items.Length; i++)
            {
                _backingList.Add(items[i]);
            }
        }

        public void RemoveAt(int row)
        {
            _backingList.RemoveRange(row * _width, _width);
        }

        public void Swap(int rowA, int rowB)
        {
            rowA *= _width;
            rowB *= _width;
            for(int i = 0; i < _width; i++)
            {
                //T temp = _backingList[rowA + i];
                //_backingList[rowA + i] = _backingList[rowB + i];
                //_backingList[rowB + i] = temp;
                _backingList.Swap(rowA + i, rowB + i);
            }
        }
    }
}
