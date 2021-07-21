using ECS;
using ECS.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    // This class is merely a subsection of the World class, so many functions will take in a typeID,
    // even though it may seem redundant due to the function GetTypeID<T>();
    // This is so that functions in the world class may call GetTypeID<T> once, and then reuse it when 
    // calling other functions here
    public class ComponentDatabase
    {
        private List<IComponentList> componentDatabase = new List<IComponentList>();

        public ComponentDatabase()
        {
            var enumerable = RegisteredComponents.OrderedTypes;
            foreach (KeyValuePair<Type, int> tIdPair in enumerable)
            {
                IComponentList list = createComponentListInstanceWithReflection(tIdPair.Key, tIdPair.Value);
                componentDatabase.Add(list);
            }
        }


        private IComponentList createComponentListInstanceWithReflection(Type t, int typeID)
        {
            Type componentListTypeIncomplete = typeof(ComponentList<>);
            Type[] typeArgs = { t };
            Type componentListType = componentListTypeIncomplete.MakeGenericType(typeArgs);
            object componentListInstance = Activator.CreateInstance(componentListType, typeID);
            return (IComponentList)componentListInstance;
        }


        public int GetTypeID<T>()
        {
            return RegisteredComponents.LookupTypeID(typeof(T));
        }

        public int GetTypeID(Type t)
        {
            return RegisteredComponents.LookupTypeID(t);
        }

        public ComponentList<T> GetComponentList<T>() where T : struct
        {
            return (ComponentList<T>)componentDatabase[GetTypeID<T>()];
        }

        public ComponentList<T> GetComponentList<T>(int typeID) where T : struct
        {
            return (ComponentList<T>)componentDatabase[typeID];
        }

        public IComponentList GetComponentList(int typeID)
        {
            return componentDatabase[typeID];
        }

        public int CreateComponent<T>(int typeID, int entityID, ref T data) where T : struct
        {
            ComponentList<T> components = (ComponentList<T>)componentDatabase[typeID];
            return components.CreateComponent(data, entityID);
        }

        public void RemoveComponent(int typeID, int componentID)
        {
            IComponentList components = componentDatabase[typeID];
            components.DestroyComponent(componentID);
        }

        public void UploadToStaticCache(int worldID)
        {
            for (int i = 0; i < componentDatabase.Count; i++)
            {
                componentDatabase[i].SendToStaticCache(worldID);
            }
        }
    }
}
