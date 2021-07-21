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
        private int[] selectedComponentTypeIDs;
        private TypeIdxPair[] selectedComponentTypeIDsSorted;
        public int Length;

        public int this[int index] { get { return selectedComponentTypeIDs[index]; } }

        public ComponentSelection(ECSWorld world, Type[] types)
        {
            selectedComponentTypeIDs = new int[types.Length];

            Length = types.Length;

            for (int i = 0; i < types.Length; i++)
            {
                selectComponentType(world, types, i);
            }

            generateSortedArray(types);
        }

        private void generateSortedArray(Type[] types)
        {
            selectedComponentTypeIDsSorted = new TypeIdxPair[types.Length];
            for (int i = 0; i < selectedComponentTypeIDsSorted.Length; i++)
            {
                selectedComponentTypeIDsSorted[i] = new TypeIdxPair(selectedComponentTypeIDs[i], i);
            }

            Array.Sort(selectedComponentTypeIDsSorted);
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

        public unsafe bool findComponentIDsUnsafe(MutableList<CompTypeIDPair> components, int* output)
        {
            int j = 0;

            for (int i = 0; i < selectedComponentTypeIDsSorted.Length; i++)
            {
                TypeIdxPair p = selectedComponentTypeIDsSorted[i];
                bool selectedComponentWasFound = false;

                for (; j < components.Count; j++)
                {
                    CompTypeIDPair p2 = components[j];
                    if (p.TypeID == p2.ComponentType)
                    {
                        output[p.InitialPos] = p2.ComponentID;
                        selectedComponentWasFound = true;
                        break;
                    }
                }

                if (!selectedComponentWasFound)
                    return false;
            }

            return true;
        }

        public bool findComponentIDs(MutableList<CompTypeIDPair> components, int[] output)
        {
            int j = 0;

            for(int i = 0; i < selectedComponentTypeIDsSorted.Length; i++)
            {
                TypeIdxPair p = selectedComponentTypeIDsSorted[i];
                bool selectedComponentWasFound = false;

                for (; j < components.Count; j++)
                {
                    CompTypeIDPair p2 = components[j];
                    if (p.TypeID == p2.ComponentType)
                    {
                        output[p.InitialPos] = p2.ComponentID;
                        selectedComponentWasFound = true;
                        break;
                    }
                }

                if (!selectedComponentWasFound)
                    return false;
            }

            return true;

            /*
            //Assumes both lists are unsorted. they no longer are
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
            */
        }
    }
}
