using ECS.CustomDataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public interface IComponentList
    {
        int CreateComponent(uint entityID);
        void DestroyComponent(int componentID);

        // Used to iterate through a component list, as it may not be a tightly packed array
        // (It used to not be, but now it is though. But in case I change it again, I have kept this function here).
        public int GetNext(int index);

        public uint GetEntityID(int compID);

        public int TypeID { get; }
        public int IterationCost { get; }
    }

    public class ComponentList<T> : IComponentList where T : struct
    {
        private MutableList<Component<T>> components = new MutableList<Component<T>>(10);
        private int typeID;

        private ComponentDatabase parentDB;

        public int ActiveComponentCount {
            get {
                return components.Count;
            }
        }

        public int TypeID {
            get {
                return typeID;
            }
        }

        public int IterationCost {
            get {
                return components.Count;
            }
        }

        public ComponentList(ComponentDatabase parent, int typeID)
        {
            this.typeID = typeID;
            this.parentDB = parent;
        }

        internal ref Component<T> this[int componentID] {
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
            if(index+1 < components.Count)
                return index + 1;
            return -1;
        }


        public int CreateComponent(uint entityID)
        {
            return CreateComponent(default, entityID);
        }

        public int CreateComponent(T data, uint entityID)
        {
            int id = components.Count;
            components.Add(new Component<T>(id, entityID, data));

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
            components.Swap(ID, components.Count - 1);
            components.RemoveAt(components.Count - 1);

            parentDB.ComponentIDChanged(components[ID].EntityID, typeID, ID);
        }

        public uint GetEntityID(int componentID)
        {
            return components[componentID].EntityID;
        }
    }
}
