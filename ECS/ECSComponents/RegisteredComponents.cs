using ECS;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ECS.Components
{
    internal struct ECSComponentattributeTypePair : IComparable<ECSComponentattributeTypePair>
    {
        public ECSComponentAttribute ECSComponentAttribute;
        public Type Type;

        public ECSComponentattributeTypePair(ECSComponentAttribute eCSComponentAttribute, Type type)
        {
            ECSComponentAttribute = eCSComponentAttribute;
            Type = type;
        }

        public int CompareTo(ECSComponentattributeTypePair other)
        {
            return ECSComponentAttribute.RegisterOrder.CompareTo(other.ECSComponentAttribute.RegisterOrder);
        }
    }

    public static class RegisteredComponents
    {
        private static List<KeyValuePair<Type, int>> componentTypesOrderedList = new List<KeyValuePair<Type, int>>();
        private static Dictionary<Type, int> componentTypesDict = new Dictionary<Type, int>();

        /// <summary>
        /// Iterate through the registered types (any struct tagged with the ECSComponentAttribute attribute) 
        /// * in order *
        /// </summary>
        public static IEnumerable<KeyValuePair<Type,int>> OrderedTypes {
            get {
                return componentTypesOrderedList;
            }
        }

        public static int Count {
            get {
                return componentTypesOrderedList.Count;
            }
        }

        static RegisteredComponents()
        {
            List<ECSComponentattributeTypePair> componentAttrTypePairs = findAllTypesWithECSComponentAttribute();
            componentAttrTypePairs.Sort();

            for(int i = 0; i < componentAttrTypePairs.Count; i++)
            {
                registerComponentType(componentAttrTypePairs[i].Type);
            }
        }

        private static List<ECSComponentattributeTypePair> findAllTypesWithECSComponentAttribute()
        {
            List<ECSComponentattributeTypePair> componentTypes = new List<ECSComponentattributeTypePair>();

            IEnumerable<Assembly> allAssemblies = getAssemblies();
            foreach(Assembly asm in allAssemblies)
            {
                try
                {
                    Type[] types = asm.GetTypes();
                    extractAllRelevantTypes(destinationList: componentTypes, types);
                }
                catch
                {
                    //do nothing.
                }
            }

            return componentTypes;
        }

        //Thankyou 3Dave for your answer on https://stackoverflow.com/questions/851248/c-sharp-reflection-get-all-active-assemblies-in-a-solution
        private static IEnumerable<Assembly> getAssemblies()
        {
            var list = new List<string>();
            var queue = new Queue<Assembly>();

            foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                queue.Enqueue(asm);
                list.Add(asm.FullName);
            }

            do
            {
                var asm = queue.Dequeue();

                yield return asm;

                try
                {
                    foreach (var reference in asm.GetReferencedAssemblies())
                    {
                        if (!list.Contains(reference.FullName))
                        {
                            queue.Enqueue(Assembly.Load(reference));
                            list.Add(reference.FullName);
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            while (queue.Count > 0);
        }

        private static void extractAllRelevantTypes(List<ECSComponentattributeTypePair> destinationList, Type[] types)
        {
            foreach (Type type in types)
            {
                object[] attributes = type.GetCustomAttributes(typeof(ECSComponentAttribute), true);
                if (attributes.Length > 0)
                {
                    destinationList.Add(
                        new ECSComponentattributeTypePair(
                            (ECSComponentAttribute)attributes[0],
                            type
                        )
                    );
                }
            }
        }

        private static void registerComponentType(Type t)
        {
            int typeID = componentTypesOrderedList.Count;
            componentTypesOrderedList.Add(new KeyValuePair<Type, int>(t, typeID));

            componentTypesDict[t] = typeID;
        }

        public static int LookupTypeID(Type t)
        {
            return componentTypesDict[t];
        }
    }
}
