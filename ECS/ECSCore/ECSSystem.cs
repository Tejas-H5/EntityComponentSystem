using ECS.CustomDataStructures;
using ECS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    //Can't call this System because of the C# System namespace.
    public abstract class ECSSystem : IECSSystem
    {
        private readonly List<IComponentList> selectedComponentLists = new List<IComponentList>();
        private readonly int[] foundComponentIDs;

        readonly private ECSWorld world;
        readonly private ComponentSelection componentSelection;

        /// <summary>
        /// Mainly used in the Iterate() function to get the selected components.
        /// </summary>
        protected ref T GetComponent<T>(int initializationOrder) where T : struct
        {
            ComponentList<T> components = StaticComponentListCache<T>.Get(world.WorldID);
            int componentID = foundComponentIDs[initializationOrder];
            return ref components[componentID].Data;
        }

        public ECSSystem(ECSWorld world, params Type[] types)
        {
            this.world = world;
            componentSelection = new ComponentSelection(this.world, types);

            for(int i = 0; i < componentSelection.Length; i++)
            {
                selectedComponentLists.Add(world.GetComponentList(componentSelection[i]));
            }

            foundComponentIDs = new int[componentSelection.Length];
        }


        public void Update(float deltaTime)
        {
            if(componentSelection.Length == 1)
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

                if (!componentSelection.findComponentIDs(entityComponents, foundComponentIDs))
                    continue;

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


        protected abstract void Iterate(float deltaTime);
    }
}
