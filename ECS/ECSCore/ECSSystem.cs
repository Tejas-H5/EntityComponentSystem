using ECS.CustomDataStructures;
using ECS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    //Can't call this System because of the C# System namespace.
    public abstract class ECSSystem
    {
        private List<IComponentList> selectedComponents = new List<IComponentList>();
        private BitArray setOfSelectedComponents = new BitArray(0);
        private int[] componentIDs;
        private ECSWorld world;

        public ECSSystem(ECSWorld world)
        {
            this.world = world;
            InitSystem(world);
        }

        /// <summary>
        /// <para>
        /// This only works if you specify initializationOrder correctly.
        /// It is a zero based index corresponding to the order in which
        /// you specified each type T for SelectComponentTypes in the Init function.
        /// </para>
        /// 
        /// Example use: 
        /// 
        /// <code>
        /// ref T tComponent = ref GetComponent&lt;T&gt;(0);
        /// </code>
        /// 
        /// </summary>
        protected ref T GetComponent<T>(int initializationOrder) where T : struct
        {
            ComponentList<T> components = (ComponentList<T>)selectedComponents[initializationOrder];
            int componentID = componentIDs[initializationOrder];
            return ref components[componentID].Data;
        }


        /// <summary>
        /// Call this function once in the overrided Init function with the types that
        /// you want this system to handle. remember the order in which you passed in the types
        /// because they will be used later.
        /// </summary>
        /// <example>
        /// <code>
        /// protected override void Init(World world)
        /// { 
        ///     SelectComponentTypes(
        ///         world,
        ///         typeof(YourComponent),
        ///         typeof(AnotherOneOfYourComponents)
        ///     );
        /// }
        /// </code>
        /// </example>
        protected void SelectComponentTypes(ECSWorld world, params Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                SelectComponentType(world, types[i]);
            }

            componentIDs = new int[types.Length];
        }

        private void SelectComponentType(ECSWorld world, Type type)
        {
            int typeID = world.GetTypeID(type);

            selectedComponents.Add(world.GetComponentList(typeID));

            addToSetOfSelectedComponents(typeID);
        }

        private void addToSetOfSelectedComponents(int typeID)
        {
            if (setOfSelectedComponents.Length < typeID + 1)
            {
                setOfSelectedComponents.Length = typeID + 1;
            }

            setOfSelectedComponents.Set(typeID, true);
        }

        public void Update(float deltaTime)
        {
            int shortestListIndex = getShortestListIndex();
            IComponentList shortestList = selectedComponents[shortestListIndex];

            int traversalComponentID = -1;
            while ((traversalComponentID = shortestList.GetNext(traversalComponentID)) != -1)
            {
                uint entityID = shortestList.GetEntityID(traversalComponentID);
                MutableList<ComponenttypeIndexPair> entityComponents = world.GetAttachedComponents(entityID);

                if (entityComponents.Count < selectedComponents.Count)
                    continue;

                componentIDs[shortestListIndex] = traversalComponentID;


                if (!findOtherSelectedComponents(traversalComponentID, entityComponents))
                    continue;

                Iterate(deltaTime);
            }
        }

        private bool findOtherSelectedComponents(int traversalComponent, MutableList<ComponenttypeIndexPair> entityComponents)
        {
            int numFound = 1;
            for (int j = 0; j < entityComponents.Count; j++)
            {
                if (j == traversalComponent)
                    continue;

                if (IsSelectedType(entityComponents[j].ComponentType))
                {
                    componentIDs[j] = traversalComponent;
                    numFound++;
                }
            }

            return numFound == selectedComponents.Count;
        }

        private int getShortestListIndex()
        {
            int shortestListIndex = 0;
            int iterCost = selectedComponents[shortestListIndex].IterationCost;

            for (int i = 1; i < selectedComponents.Count; i++)
            {
                if (selectedComponents[i].IterationCost < iterCost)
                {
                    shortestListIndex = i;
                }
            }

            return shortestListIndex;
        }

        private bool IsSelectedType(int typeID)
        {
            if (typeID >= setOfSelectedComponents.Length)
                return false;

            return setOfSelectedComponents.Get(typeID);
        }

        /// <summary>
        /// This function must call SelectComonentTypes.
        /// </summary>
        protected abstract void InitSystem(ECSWorld world);


        /// <summary>
        /// Use the GetComponent
        /// </summary>
        protected abstract void Iterate(float deltaTime);
    }
}
