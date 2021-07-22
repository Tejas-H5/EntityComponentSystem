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

        private List<int> entityIDs = new List<int>();
        private List<bool> destroyedFlags = new List<bool>();
        private MutableList<T> components = new MutableList<T>(16);
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

        public ref T this[int componentID] {
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
                if (destroyedFlags[i])
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
                destroyedFlags[pooledID] = false;

                if (pooledID > backOfList)
                    backOfList = pooledID;

                return pooledID;
            }


            //Atomic
            int id = components.Count;
            components.Add(data);
            entityIDs.Add(entityID);
            destroyedFlags.Add(false);


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
            return !destroyedFlags[ID];
        }


        public void DestroyComponent(int componentIDs)
        {
            entityIDs[componentIDs] = ECSConstants.InvalidEntityID;
            destroyedFlags[componentIDs] = true;
            deletedList.Enqueue(componentIDs);

            if (componentIDs == backOfList)
            {
                while (backOfList > 0 && destroyedFlags[backOfList])
                {
                    backOfList--;
                }
            }
        }

        public int GetEntityID(int componentID)
        {
            return entityIDs[componentID];
        }

        public void SendToStaticCache(int worldID)
        {
            StaticComponentListCache<T>.Set(worldID, this);
        }
    }
}