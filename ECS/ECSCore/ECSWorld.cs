using ECS.CustomDataStructures;
using ECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    internal struct ComponenttypeIndexPair
    {
        public int ComponentType;
        public int ComponentID;

        public ComponenttypeIndexPair(int componentType, int componentID)
        {
            ComponentType = componentType;
            ComponentID = componentID;
        }
    }


    public class ECSWorld
    {
        private uint nextEntityID = 0;
        private Queue<uint> freeList = new Queue<uint>();
        private Dictionary<uint, MutableList<ComponenttypeIndexPair>> entities = new Dictionary<uint, MutableList<ComponenttypeIndexPair>>();
        private ComponentDatabase componentDatabase;

        public int EntityCount {
            get {
                return entities.Count - freeList.Count;
            }
        }

        public ECSWorld()
        {
            componentDatabase = new ComponentDatabase();
        }

        public uint CreateEntity()
        {
            if (freeList.Count > 0)
            {
                return freeList.Dequeue();
            }

            uint newEntity = nextNewEntityID();
            entities[newEntity] = new MutableList<ComponenttypeIndexPair>(10);

            return newEntity;
        }

        private uint nextNewEntityID()
        {
            uint id = nextEntityID;
            nextEntityID++;
            return id;
        }


        public void DestroyEntity(uint entityID)
        {
            if (!entities.ContainsKey(entityID))
            {
                return;
            }

            MutableList<ComponenttypeIndexPair> entityComponents = GetAttachedComponents(entityID);

            for (int i = 0; i < entityComponents.Count; i++)
            {
                ComponenttypeIndexPair cip = entityComponents[i];
                componentDatabase.DestroyComponent(cip.ComponentType, cip.ComponentID);
            }
            entityComponents.Clear();

            freeList.Enqueue(entityID);
        }

        public void AddComponent<T>(uint entityID, T data) where T : struct
        {
            int typeID = componentDatabase.GetTypeID<T>();

            int componentID = componentDatabase.CreateComponent<T>(typeID, entityID, ref data);
            entities[entityID].Add(new ComponenttypeIndexPair(typeID, componentID));
        }

        public void RemoveComponent<T>(int componentID, uint entityID) where T : struct
        {
            int typeID = componentDatabase.GetTypeID<T>();
            MutableList<ComponenttypeIndexPair> components = entities[entityID];

            int pos = findComponentOfType(components, typeID);

            if (pos == -1)
            {
                throw new TypeAccessException("The entity " + entityID + " does not have a component of type " + typeof(T));
            }

            swapToBackAndRemove(components, pos);

            componentDatabase.DestroyComponent(typeID, componentID);
        }

        private int findComponentOfType(MutableList<ComponenttypeIndexPair> components, int typeID)
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].ComponentType == typeID)
                {
                    return i;
                }
            }

            return -1;
        }

        private static void swapToBackAndRemove(MutableList<ComponenttypeIndexPair> components, int pos)
        {
            components.Swap(pos, components.Count - 1);
            components.RemoveAt(components.Count - 1);
        }

        public ComponentList<T> GetComponentList<T>(int typeID) where T : struct
        {
            return componentDatabase.GetComponentList<T>(typeID);
        }

        public int GetTypeID<T>()
        {
            return componentDatabase.GetTypeID<T>();
        }

        public int GetTypeID(Type t)
        {
            return componentDatabase.GetTypeID(t);
        }

        public IComponentList GetComponentList(int typeID)
        {
            return componentDatabase.GetComponentList(typeID);
        }

        internal MutableList<ComponenttypeIndexPair> GetAttachedComponents(uint entityID)
        {
            return entities[entityID];
        }


        /// <summary>
        /// Will throw an exception if this entity does not in fact have a T in it's posession
        /// </summary>
        public ref T GetComponentFromEntity<T>(uint entityID) where T : struct
        {
            int typeID = RegisteredComponents.LookupTypeID(typeof(T));
            MutableList<ComponenttypeIndexPair> components = GetAttachedComponents(entityID);

            int componentID = -1;

            for (int i = 0; i < components.Count; i++)
            {
                if(components[i].ComponentType == typeID)
                {
                    componentID = components[i].ComponentID;
                    break;
                }
            }

            return ref componentDatabase.GetComponentList<T>(typeID)[componentID].Data;
        }
    }
}
