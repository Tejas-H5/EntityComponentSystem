using ECS.CustomDataStructures;
using ECS;
using ECS.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public class ECSWorld
    {
        private static int nextWorldID = 0;

        private Queue<int> destroyedList = new Queue<int>();
        private List<bool> isDestroyed = new List<bool>();

        private List<MutableList<CompTypeIDPair>> entityList = new List<MutableList<CompTypeIDPair>>();

        private ComponentDatabase componentDatabase;

        private List<IECSListner> subscribedListeners = new List<IECSListner>();

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

            componentDatabase.UploadToStaticCache(WorldID);
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
            entityList.Add(new MutableList<CompTypeIDPair>(3));
            isDestroyed.Add(false);

            for (int i = 0; i < subscribedListeners.Count; i++)
            {
                subscribedListeners[i].OnAddEntity(entityList[newEntity], newEntity);
            }

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

            MutableList<CompTypeIDPair> entityComponents = GetAttachedComponents(entityID);


            for (int i = 0; i < subscribedListeners.Count; i++)
            {
                subscribedListeners[i].OnAddEntity(entityComponents, entityID);
            }


            for (int i = 0; i < entityComponents.Count; i++)
            {
                CompTypeIDPair cip = entityComponents[i];
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

            MutableList<CompTypeIDPair> entityComponents = entityList[entityID];

#if DEBUG
            int i = findComponentOfType(entityComponents, typeID);
            if (i != -1)
                throw new Exception("This component already exists on this type");
#endif
            int indexIntoComponentList = entityComponents.Count;
            entityComponents.Add(new CompTypeIDPair(typeID, componentID));

            for(int i = 0; i < subscribedListeners.Count; i++)
            {
                subscribedListeners[i].OnAddComponent(entityComponents, entityID, indexIntoComponentList);
            }
        }

        public void RemoveComponent<T>(int componentID, int entityID) where T : struct
        {
            int typeID = componentDatabase.GetTypeID<T>();
            MutableList<CompTypeIDPair> components = entityList[entityID];

            int indexIntoComponents = findComponentOfType(components, typeID);

            if (indexIntoComponents == -1)
            {
                throw new TypeAccessException("The entity " + entityID + " does not have a component of type " + typeof(T));
            }

            for (int i = 0; i < subscribedListeners.Count; i++)
            {
                subscribedListeners[i].OnRemoveComponent(components, entityID, indexIntoComponents);
            }

            swapToBackAndRemove(components, indexIntoComponents);

            componentDatabase.DestroyComponent(typeID, componentID);
        }

        private int findComponentOfType(MutableList<CompTypeIDPair> components, int typeID)
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

        private static void swapToBackAndRemove(MutableList<CompTypeIDPair> components, int pos)
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

        internal MutableList<CompTypeIDPair> GetAttachedComponents(int entityID)
        {
#if DEBUG
            if (entityID < 0 || entityID >= entityList.Count)
            {
                int i = 5;
                throw new IndexOutOfRangeException();
            }
#endif
            return entityList[entityID];
        }


        /// <summary>
        /// Will throw an exception if this entity does not in fact have a T in it's posession
        /// </summary>
        public ref T GetComponentFromEntity<T>(int entityID) where T : struct
        {
            int typeID = RegisteredComponents.LookupTypeID(typeof(T));
            MutableList<CompTypeIDPair> components = GetAttachedComponents(entityID);

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
            MutableList<CompTypeIDPair> components = GetAttachedComponents(entityID);
            for(int i = 0; i < components.Count; i++)
            {
                if (components[i].ComponentType == typeID)
                    components[i].ComponentID = newComponentID;
            }
        }

        public bool HasComponent<T>(int entityID)
        {
            int typeID = RegisteredComponents.LookupTypeID(typeof(T));
            MutableList<CompTypeIDPair> components = GetAttachedComponents(entityID);
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].ComponentType == typeID)
                    return true;
            }

            return false;
        }

        public void SubscribeListener(IECSListner listener)
        {
            subscribedListeners.Add(listener);
        }

        public void UnsubscribeListener(IECSListner listener)
        {
            subscribedListeners.Remove(listener);
        }
    }
}
