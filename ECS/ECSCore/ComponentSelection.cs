using ECS.CustomDataStructures;
using ECS.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace ECS
{
    internal struct TypeIdxPair : IComparable<TypeIdxPair>
    {
        public int TypeID;
        public int InitialPos;

        public TypeIdxPair(int typeID, int initialPos)
        {
            TypeID = typeID;
            InitialPos = initialPos;
        }

        public int CompareTo(TypeIdxPair other)
        {
            return TypeID.CompareTo(other.TypeID);
        }
    }

    public class ComponentSelection
    {
        private readonly int[] selectedComponentTypeIDs;
        private readonly int[] selectedComponentTypeIDsSorted;
        private readonly int[] sortedInitialPositions;
        public readonly int Length;

        public int this[int index] { get { return selectedComponentTypeIDs[index]; } }

        public ComponentSelection(ECSWorld world, Type[] types)
        {
            selectedComponentTypeIDs = new int[types.Length];

            Length = types.Length;

            for (int i = 0; i < types.Length; i++)
            {
                selectComponentType(world, types, i);
            }

            selectedComponentTypeIDsSorted = new int[types.Length];
            sortedInitialPositions = new int[types.Length];
            generateSortedArray(types);
        }

        private void generateSortedArray(Type[] types)
        {
            for (int i = 0; i < selectedComponentTypeIDsSorted.Length; i++)
            {
                selectedComponentTypeIDsSorted[i] = selectedComponentTypeIDs[i];
                sortedInitialPositions[i] = i;
            }

            Array.Sort(selectedComponentTypeIDsSorted, sortedInitialPositions);
        }

        private void selectComponentType(ECSWorld world, Type[] types, int i)
        {
            int typeID = world.GetTypeID(types[i]);
            selectedComponentTypeIDs[i] = typeID;
        }

        public int selectedComponentIndex(int type)
        {
            for (int i = 0; i < selectedComponentTypeIDs.Length; i++)
            {
                if (selectedComponentTypeIDs[i] == type)
                {
                    return i;
                }
            }
            return -1;
        }

        public unsafe bool findComponentIDs(MutableList<CompTypeIDPair> components, int[] output)
        {
            //*
            int selIndex = 0;
            int selType = selectedComponentTypeIDsSorted[0];

            fixed(CompTypeIDPair* fixedPtr = &components[0])
            {
                CompTypeIDPair* ptr = fixedPtr;
                for (int i = 0; i < components.Count; i++, ptr++)
                {
                    if(ptr->ComponentType == selType)
                    {
                        output[sortedInitialPositions[selIndex]] = ptr->ComponentID;

                        selIndex++;

                        if (selIndex == this.Length)
                            return true;
                        selType = selectedComponentTypeIDsSorted[selIndex];
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Deprecated, as it assumes both lists are unsorted. they no longer are
        /// </summary>
        public bool findComponentIDsUnsorted(MutableList<CompTypeIDPair> components, int[] output)
        {
            for (int i = 0; i < selectedComponentTypeIDs.Length; i++)
            {
                int selType = selectedComponentTypeIDs[i];
                bool selectedComponentWasFound = false;
                for (int j = 0; j < components.Count; j++)
                {
                    ref CompTypeIDPair c = ref components[j];
                    if (c.ComponentType == selType)
                    {
                        selectedComponentWasFound = true;
                        output[i] = c.ComponentID;
                        break;
                    }
                }

                if (!selectedComponentWasFound)
                    return false;
            }

            return true;
        }

    }
}
