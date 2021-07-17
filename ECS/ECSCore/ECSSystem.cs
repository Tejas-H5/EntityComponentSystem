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
        private int[] selectedComponentTypeIDs;
        private int[] componentIDs;
        private ECSWorld world;

        public ECSSystem(ECSWorld world)
        {
            this.world = world;
            InitSystem();
        }

        

        /// <summary>
        /// <para>
        /// Mainly used in the Iterate() function to get the selected components.
        /// </para>
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
        /// you want this system to handle. Remember the order in which you passed in the types
        /// because it is needed to use the function
        /// <c>GetComponent&lt;T&gt;(int initializationOrder)</c> properly.
        /// </summary>
        /// <example>
        /// <code>
        /// protected override void Init(World world)
        /// { 
        ///     SelectComponentTypes(
        ///         typeof(YourComponent),
        ///         typeof(AnotherOneOfYourComponents)
        ///     );
        /// }
        /// </code>
        /// </example>
        protected void SelectComponentTypes(params Type[] types)
        {
            selectedComponentTypeIDs = new int[types.Length];
            componentIDs = new int[types.Length];

            for (int i = 0; i < types.Length; i++)
            {
                int typeID = world.GetTypeID(types[i]);

                selectedComponents.Add(world.GetComponentList(typeID));
                selectedComponentTypeIDs[i] = typeID;
            }
        }

        public void Update(float deltaTime)
        {
            if(selectedComponentTypeIDs.Length == 1)
            {
                updateSingleComponent(deltaTime);
            }
            else
            {
                updateMultiComponent(deltaTime);
            }
        }

        private void updateSingleComponent(float deltaTime)
        {
            IComponentList onlyList = selectedComponents[0];
            int traversalComponentID = -1;
            while ((traversalComponentID = onlyList.GetNext(traversalComponentID)) != -1)
            {
                componentIDs[0] = traversalComponentID;
                Iterate(deltaTime);
            }
        }

        private void updateMultiComponent(float deltaTime)
        {
            int shortestListIndex = getShortestListIndex();
            IComponentList shortestList = selectedComponents[shortestListIndex];

            int traversalComponentID = -1;
            int traversalComponentTypeID = shortestList.TypeID;
            while ((traversalComponentID = shortestList.GetNext(traversalComponentID)) != -1)
            {
                uint entityID = shortestList.GetEntityID(traversalComponentID);
                MutableList<ComponenttypeIndexPair> entityComponents = world.GetAttachedComponents(entityID);

                if (entityComponents.Count < selectedComponents.Count)
                    continue;

                componentIDs[shortestListIndex] = traversalComponentID;

                if (!findOtherSelectedComponents(traversalComponentTypeID, entityComponents))
                    continue;

                Iterate(deltaTime);
            }
        }

        /// <summary>
        /// Use the GetComponent function here to get the components you selected in the Init function
        /// </summary>
        protected abstract void Iterate(float deltaTime);

        private bool findOtherSelectedComponents(int traversalComponentTypeID, MutableList<ComponenttypeIndexPair> entityComponents)
        {
            int numFound = 1;

            for (int j = 0; j < entityComponents.Count; j++)
            {
                ref ComponenttypeIndexPair compIndexPair = ref entityComponents[j];

                if (compIndexPair.ComponentType == traversalComponentTypeID)
                    continue;

                int selectedComponent = findSelectedTypeIndex(compIndexPair.ComponentType);
                if (selectedComponent != -1)
                {
                    componentIDs[selectedComponent] = compIndexPair.ComponentID;
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
                int thisIterCost = selectedComponents[i].IterationCost;
                if (thisIterCost < iterCost)
                {
                    shortestListIndex = i;
                    iterCost = thisIterCost;
                }
            }

            return shortestListIndex;
        }

        private int findSelectedTypeIndex(int typeID)
        {
            for(int i = 0; i < selectedComponentTypeIDs.Length; i++)
            {
                if (selectedComponentTypeIDs[i] == typeID)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// This function must call SelectComonentTypes.
        /// </summary>
        protected abstract void InitSystem();
    }
}
