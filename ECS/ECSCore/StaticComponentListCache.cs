using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    /// <summary>
    /// An optimzation for the GetComponent method in ECSSystem
    /// </summary>
    public static class StaticComponentListCache<T> where T : struct
    {
        private static List<ComponentList<T>> componentListList = new List<ComponentList<T>>();

        public static void Set(int worldID, ComponentList<T> instance)
        {
            ensureSize(worldID);

            componentListList[worldID] = instance;
        }

        private static void ensureSize(int worldID)
        {
            while (componentListList.Count - 1 < worldID)
            {
                componentListList.Add(null);
            }
        }

        public static ComponentList<T> Get(int worldID)
        {
            return componentListList[worldID];
        }
    }
}
