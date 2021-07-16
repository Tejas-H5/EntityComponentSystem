using ECS.CustomDataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestECSUnitTests.CustomDataStructures
{
    struct Vec3
    {
        public float x, y, z;

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [TestClass]
    public class MutableListTest
    {
        [TestMethod]
        public void Add_ShouldPutThingsOnEndOfList()
        {
            MutableList<int> list = CreateListAscending(10);

            Assert.IsTrue(list.Count == 10);

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(list[i] == i);
            }
        }

        private static MutableList<int> CreateListAscending(int numElements)
        {
            MutableList<int> list = new MutableList<int>(numElements);

            for (int i = 0; i < numElements; i++)
            {
                list.Add(i);
            }

            return list;
        }

        [TestMethod]
        public void Add_ShouldWorkWhenBackingArrayIsZero()
        {
            MutableList<int> list = new MutableList<int>(0);

            for (int i = 0; i < 10; i++)
            {
                list.Add(i);
            }

            Assert.IsTrue(list.Count == 10);

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(list[i] == i);
            }
        }


#if DEBUG

        [TestMethod]
        public void Add_ShouldntReallocateIfNotNeeded()
        {
            MutableList<int> list = CreateListAscending(10);

            Assert.IsTrue(list.Count == 10);

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(list[i] == i);
            }

            Assert.IsTrue(list.Reallocations == 0);
        }
#endif

#if DEBUG

        [TestMethod]
        public void Add_ShouldReallocateIfNeeded()
        {
            MutableList<int> list = CreateListAscending(10);
            list.Add(1);

            Assert.IsTrue(list.Count == 11);

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(list[i] == i);
            }

            Assert.IsTrue(list[10] == 1);

            Assert.IsTrue(list.Reallocations == 1);
        }
#endif

        [TestMethod]
        public void RemoveAt_ShouldRemoveFromStarOfList()
        {
            MutableList<int> list = CreateListAscending(10);


            for (int i = 0; i < 10; i++)
            {
                list.RemoveAt(0);
            }

            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void RemoveAt_ShouldRemoveFromEndOfList()
        {
            MutableList<int> list = CreateListAscending(10);

            for (int i = 0; i < 10; i++)
            {
                list.RemoveAt(list.Count - 1);
            }

            Assert.IsTrue(list.Count == 0);
        }


        [TestMethod]
        public void RemoveAt_ShouldRemoveCorrectItems()
        {
            MutableList<int> list = CreateListAscending(10);

            list.RemoveAt(0);
            list.RemoveAt(5);
            list.RemoveAt(7);

            int[] expected = new int[] { 1, 2, 3, 4, 5, 7, 8 };

            AssertListsEqual(list, expected);
        }

        private static void AssertListsEqual(MutableList<int> list, IList<int> expected)
        {
            Assert.IsTrue(list.Count == expected.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.IsTrue(list[i] == expected[i]);
            }
        }


        [TestMethod]
        public void RemoveAt_ShouldMaintainListOrder()
        {
            MutableList<int> list = CreateListAscending(10);

            for (int i = 0; i < list.Count; i += 2)
            {
                list.RemoveAt(i);
                i--;
            }

            for (int i = 0; i < list.Count; i++)
            {
                Assert.IsTrue(list[i] == (i * 2 + 1));
            }
        }

        [TestMethod]
        public void Insert_ShouldInsertThings()
        {
            MutableList<int> list = CreateList<int>(5);

            list.Insert(0, 10);
            list.Insert(list.Count, 20);

            list.Insert(list.Count / 2, 30);

            int[] expected = { 10, 0, 0, 30, 0, 0, 0, 20 };

            AssertListsEqual(list, expected);
        }

        MutableList<T> CreateList<T>(int size)
        {
            MutableList<T> list = new MutableList<T>(size);

            for (int i = 0; i < size; i++)
            {
                list.Add(default(T));
            }

            return list;
        }

        [TestMethod]
        public void Swap_ShouldSwapThings()
        {
            MutableList<int> list = CreateListAscending(5);

            for (int i = 0; i < list.Count; i++)
            {
                list.Swap(i, list.Count - i - 1);
            }

            int[] expected = { 0, 1, 2, 3, 4 };

            AssertListsEqual(list, expected);
        }

        [TestMethod]
        public void Clear_ShouldRemoveAll()
        {
            MutableList<int> list = CreateListAscending(5);

            list.Clear();

            int[] expected = { };

            AssertListsEqual(list, expected);
        }

#if DEBUG
        [TestMethod]
        public void Clear_ShouldntReallocate()
        {
            MutableList<int> list = CreateListAscending(5);

            for(int i = 0; i < 20; i++)
            {
                RefreshList(list);
            }
            
            int[] expected = { 0, 1, 2, 3, 4 };

            AssertListsEqual(list, expected);

            Assert.IsTrue(list.Reallocations == 0);
        }
#endif

        private static void RefreshList(MutableList<int> list)
        {
            int count = list.Count;
            list.Clear();

            for (int i = 0; i < count; i++)
            {
                list.Add(i);
            }
        }

#if DEBUG
        [TestMethod]
        public void Trim_ShouldReallocate()
        {
            MutableList<int> list = CreateListAscending(5);

            for(int i = 0; i < 4; i++)
            {
                list.Add(1);
                list.TrimToSize();
            }


            int[] expected = { 0, 1, 2, 3, 4, 1, 1, 1, 1 };

            AssertListsEqual(list, expected);

            Assert.IsTrue(list.Reallocations == 8);
        }
#endif

#if DEBUG
        [TestMethod]
        public void Trim_ShouldntReallocateToLessThanInitialCapacity()
        {
            MutableList<int> list = CreateListAscending(5);

            for (int i = 0; i < 4; i++)
            {
                list.RemoveAt(0);
                list.TrimToSize();
            }


            int[] expected = {4};

            AssertListsEqual(list, expected);

            Assert.IsTrue(list.Reallocations == 0);
        }
#endif
    }
}
