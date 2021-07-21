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
        protected List<IComponentList> selectedComponentLists = new List<IComponentList>();
        protected int[] selectedComponentTypeIDs;

        private int[] foundComponentIDs;
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
            ComponentList<T> components = StaticComponentListCache<T>.Get(world.WorldID);
            int componentID = foundComponentIDs[initializationOrder];
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
            foundComponentIDs = new int[types.Length];
            selectedComponentLists.Capacity = types.Length;

            for (int i = 0; i < types.Length; i++)
            {
                selectComponentType(types, i);
            }
        }

        private void selectComponentType(Type[] types, int i)
        {
            int typeID = world.GetTypeID(types[i]);

            selectedComponentLists.Add(world.GetComponentList(typeID));
            selectedComponentTypeIDs[i] = typeID;
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
            IComponentList onlyList = selectedComponentLists[0];
            int traversalComponentID = -1;

            while ((traversalComponentID = onlyList.GetNext(traversalComponentID)) != -1)
            {
                foundComponentIDs[0] = traversalComponentID;
                Iterate(deltaTime);
            }
        }

        private void updateMultiComponent(float deltaTime)
        {
            int shortestListIndex = getShortestListIndex();
            IComponentList shortestList = selectedComponentLists[shortestListIndex];

            int traversalComponentID = -1;
            int traversalComponentTypeID = shortestList.TypeID;
            while ((traversalComponentID = shortestList.GetNext(traversalComponentID)) != -1)
            {
                int entityID = shortestList.GetEntityID(traversalComponentID);
                MutableList<CompTypeIDPair> entityComponents = world.GetAttachedComponents(entityID);

                if (!findOtherSelectedComponents(traversalComponentTypeID, entityComponents))
                    continue;

                foundComponentIDs[shortestListIndex] = traversalComponentID;

                Iterate(deltaTime);
            }
        }

        private int getShortestListIndex()
        {
            int shortestListIndex = 0;
            int iterCost = selectedComponentLists[shortestListIndex].IterationCost;

            for (int i = 1; i < selectedComponentLists.Count; i++)
            {
                int thisIterCost = selectedComponentLists[i].IterationCost;
                if (thisIterCost < iterCost)
                {
                    shortestListIndex = i;
                    iterCost = thisIterCost;
                }
            }

            return shortestListIndex;
        }

        private bool findOtherSelectedComponents(int traversalComponentTypeID, MutableList<CompTypeIDPair> entityComponents)
        {
            if (entityComponents.Count < selectedComponentLists.Count)
                return false;

            for (int j = 0; j < selectedComponentTypeIDs.Length; j++)
            {
                if (j == traversalComponentTypeID)
                    continue;

                int thisComponentType = selectedComponentTypeIDs[j];
                int thisComponentID = -1;

                for(int k = 0; k < entityComponents.Count; k++)
                {
                    ref CompTypeIDPair compIndexPair = ref entityComponents[k];
                    if (compIndexPair.ComponentType == thisComponentType)
                    {
                        thisComponentID = compIndexPair.ComponentID;
                        break;
                    }
                }

                bool aSingleComponentWasntFound = thisComponentID == -1;
                if (aSingleComponentWasntFound)
                    return false;

                foundComponentIDs[j] = thisComponentID;
            }

            return true;
        }


        /// <summary>
        /// This function must call SelectComonentTypes.
        /// </summary>
        protected abstract void InitSystem();

        /// <summary>
        /// Use the GetComponent function here to get the components you selected in the Init function
        /// </summary>
        protected abstract void Iterate(float deltaTime);
    }
}
