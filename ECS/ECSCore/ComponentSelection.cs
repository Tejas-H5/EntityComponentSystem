using ECS.CustomDataStructures;
using ECS.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public class ComponentSelection
    {
        private readonly int[] selectedComponentTypeIDs;
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


        public bool findComponentIDs(MutableList<CompTypeIDPair> components, int[] output)
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
