using ECS.CustomDataStructures;
using ECS.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public interface IComponentList
    {
        int CreateComponent(int entityID);
        void DestroyComponent(int componentID);

        // Used to iterate through a component list, as it may not be a tightly packed array
        // (which it isn't in the current implementation)
        public int GetNext(int index);

        public int GetEntityID(int compID);

        public void SendToStaticCache(int worldID);

        public int TypeID { get; }
        public int IterationCost { get; }
    }

    public class ComponentList<T> : IComponentList where T : struct
    {
        private Queue<int> deletedList = new Queue<int>();

        //Note to future dev(s): Replacing Component<T> with <T> and using a Struct of Arrays approach
        //does not improve benchmarks and makes readability worse, so don't do it
        private MutableList<Component<T>> components = new MutableList<Component<T>>(10);
        private int backOfList = 0;

        public int ActiveComponentCount {
            get {
                return components.Count - deletedList.Count;
            }
        }

        private int typeID;
        public int TypeID {
            get {
                return typeID;
            }
        }

        public int IterationCost {
            get {
                return backOfList;
            }
        }

        public ComponentList(int typeID)
        {
            this.typeID = typeID;
        }

        public ref Component<T> this[int componentID] {
            get {
                return ref components[componentID];
            }
        }

        /// <summary>
        /// Returns the index of the next valid component starting from index.
        /// if the end of the list is reached, it returns -1.
        /// </summary>
        public int GetNext(int index)
        {
            for (int i = index + 1; i <= backOfList; i++)
            {
                if (components[i].IsDestroyed)
                    continue;
                return i;
            }

            return -1;
        }


        public int CreateComponent(int entityID)
        {
            return CreateComponent(default, entityID);
        }

        public int CreateComponent(T data, int entityID)
        {
            if (deletedList.Count > 0)
            {
                int pooledID = deletedList.Dequeue();
                components[pooledID].IsDestroyed = false;
                return pooledID;
            }

            int id = components.Count;
            components.Add(new Component<T>(id, entityID, data));

            if (id > backOfList)
                backOfList = id;

            return id;
        }


        public bool IsValidComponent(int ID)
        {
            if (ID < 0 || ID >= components.Count)
                return false;

            if (isValidComponentID(ID))
                return false;

            return true;
        }

        private bool isValidComponentID(int ID)
        {
            return components[ID].EntityID == ECSConstants.InvalidEntityID;
        }


        public void DestroyComponent(int ID)
        {
            components[ID].EntityID = ECSConstants.InvalidEntityID;
            components[ID].IsDestroyed = true;
            deletedList.Enqueue(ID);

            if (ID == backOfList)
            {
                while (backOfList > 0 && components[backOfList].IsDestroyed)
                {
                    backOfList--;
                }
            }
        }

        public int GetEntityID(int componentID)
        {
            return components[componentID].EntityID;
        }

        public void SendToStaticCache(int worldID)
        {
            StaticComponentListCache<T>.Set(worldID, this);
        }
    }
}