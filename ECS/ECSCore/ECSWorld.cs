﻿using ECS.CustomDataStructures;
using ECS;
using ECS.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public partial class ECSWorld
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

        public EntityBuilder[] CreateEntities(int number)
        {
            EntityBuilder[] entities = new EntityBuilder[number];

            for(int i = 0; i < number; i++)
            {
                entities[i] = CreateEntity();
            }

            return entities;
        }


        public EntityBuilder CreateEntity()
        {
            if (destroyedList.Count > 0)
            {
                int pooledEntity = destroyedList.Dequeue();
                isDestroyed[pooledEntity] = false;
                return new EntityBuilder(this, pooledEntity);
            }

            int newEntity = entityList.Count;
            entityList.Add(new MutableList<CompTypeIDPair>(3));
            isDestroyed.Add(false);

            return new EntityBuilder(this, newEntity);
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

            invokeEntityDestroyedEvent(entityID, entityComponents);

            for (int i = 0; i < entityComponents.Count; i++)
            {
                CompTypeIDPair cip = entityComponents[i];
                componentDatabase.RemoveComponent(cip.ComponentType, cip.ComponentID);
            }

            entityComponents.Clear();

            destroyedList.Enqueue(entityID);
            isDestroyed[entityID] = true;
        }

        public void AddComponent<T>(int entityID, T data) where T : struct
        {
            addComponent(entityID, data, true);
        }

        private void addComponentNoEvent<T>(int entityID, T data) where T : struct
        {
            addComponent(entityID, data, false);
        }

        private void addComponent<T>(int entityID, T data, bool sendEvent) where T : struct
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
            entityComponents.Sort();

            if (sendEvent)
            {
                invokeComponentAddedEvent(entityID, entityComponents, indexIntoComponentList);
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

            invokeComponentRemovedEvent(entityID, components, indexIntoComponents);

            components.Swap(indexIntoComponents, components.Count - 1);
            components.RemoveAt(components.Count - 1);

            componentDatabase.RemoveComponent(typeID, componentID);
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

            return ref componentDatabase.GetComponentList<T>(typeID)[componentID];
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

        private void invokeEntityCreatedEvent(int newEntityID)
        {
            for (int i = 0; i < subscribedListeners.Count; i++)
            {
                subscribedListeners[i].OnEntityCreated(entityList[newEntityID], newEntityID);
            }
        }

        private void invokeEntityDestroyedEvent(int entityID, MutableList<CompTypeIDPair> entityComponents)
        {
            for (int i = 0; i < subscribedListeners.Count; i++)
            {
                subscribedListeners[i].OnEntityRemoved(entityComponents, entityID);
            }
        }

        private void invokeComponentRemovedEvent(int entityID, MutableList<CompTypeIDPair> components, int indexIntoComponents)
        {
            for (int i = 0; i < subscribedListeners.Count; i++)
            {
                subscribedListeners[i].OnRemoveComponent(components, entityID, indexIntoComponents);
            }
        }

        private void invokeComponentAddedEvent(int entityID, MutableList<CompTypeIDPair> entityComponents, int indexIntoComponentList)
        {
            for (int i = 0; i < subscribedListeners.Count; i++)
            {
                subscribedListeners[i].OnAddComponent(entityComponents, entityID, indexIntoComponentList);
            }
        }
    }
}
