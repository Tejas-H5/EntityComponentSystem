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
        private static int nextWorldID = 0;

        private Queue<int> destroyedList = new Queue<int>();
        private List<bool> isDestroyed = new List<bool>();

        private List<MutableList<ComponenttypeIndexPair>> entityList = new List<MutableList<ComponenttypeIndexPair>>();

        private ComponentDatabase componentDatabase;

        public int EntityCount {
            get {
                return entityList.Count - destroyedList.Count;
            }
        }

        public readonly int WorldID;

        public ECSWorld()
        {
            componentDatabase = new ComponentDatabase();
            WorldID = nextWorldID;
            nextWorldID++;
        }

        public int[] CreateEntities(int number)
        {
            int[] entities = new int[number];

            for(int i = 0; i < number; i++)
            {
                entities[i] = CreateEntity();
            }

            return entities;
        }


        public int CreateEntity()
        {
            if (destroyedList.Count > 0)
            {
                int pooledEntity = destroyedList.Dequeue();
                isDestroyed[pooledEntity] = false;
                return pooledEntity;
            }

            int newEntity = entityList.Count;
            entityList.Add(new MutableList<ComponenttypeIndexPair>(3));
            isDestroyed.Add(false);

            return newEntity;
        }


        public void DestroyEntities(IList<int> entities)
        {
            for(int i = 0; i < entities.Count; i++)
            {
                DestroyEntity(entities[i]);
            }
        }


        public void DestroyEntity(int entityID)
        {
            if (entityID >= entityList.Count || entityID < 0)
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

            destroyedList.Enqueue(entityID);
            isDestroyed[entityID] = true;
        }


        public void AddComponent<T>(int entityID, T data) where T : struct
        {
            int typeID = componentDatabase.GetTypeID<T>();

            int componentID = componentDatabase.CreateComponent<T>(typeID, entityID, ref data);

            MutableList<ComponenttypeIndexPair> entityComponents = entityList[entityID];

#if DEBUG
            int i = findComponentOfType(entityComponents, typeID);
            if (i != -1)
                throw new Exception("This component already exists on this type");
#endif

            entityComponents.Add(new ComponenttypeIndexPair(typeID, componentID));
        }

        public void RemoveComponent<T>(int componentID, int entityID) where T : struct
        {
            int typeID = componentDatabase.GetTypeID<T>();
            MutableList<ComponenttypeIndexPair> components = entityList[entityID];

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

        internal MutableList<ComponenttypeIndexPair> GetAttachedComponents(int entityID)
        {
            return entityList[entityID];
        }


        /// <summary>
        /// Will throw an exception if this entity does not in fact have a T in it's posession
        /// </summary>
        public ref T GetComponentFromEntity<T>(int entityID) where T : struct
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

        internal void ComponentIDChanged(int entityID, int typeID, int newComponentID)
        {
            MutableList<ComponenttypeIndexPair> components = GetAttachedComponents(entityID);
            for(int i = 0; i < components.Count; i++)
            {
                if (components[i].ComponentType == typeID)
                    components[i].ComponentID = newComponentID;
            }
        }

        public bool HasComponent<T>(int entityID)
        {
            int typeID = RegisteredComponents.LookupTypeID(typeof(T));
            MutableList<ComponenttypeIndexPair> components = GetAttachedComponents(entityID);
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].ComponentType == typeID)
                    return true;
            }

            return false;
        }
    }
}
