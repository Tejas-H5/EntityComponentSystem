using ECS.Components;
using ECS.CustomDataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public abstract class ECSRelevantComponentListner : IECSListner
    {
        protected int[] foundComponentIDs;

        protected ECSWorld world;
        protected ComponentSelection componentSelection;

        public ECSRelevantComponentListner(ECSWorld world, ComponentSelection selection) {
            this.world = world;
            componentSelection = selection;
            foundComponentIDs = new int[selection.Length];
        }

        public void OnEntityCreated(MutableList<CompTypeIDPair> components, int entityID)
        {
            addEntityIfItHasSelectedComponents(components, entityID);
        }

        private void addEntityIfItHasSelectedComponents(MutableList<CompTypeIDPair> components, int entityID)
        {
            if (componentSelection.findComponentIDs(components, foundComponentIDs))
            {
                OnAddRelevantEntity(foundComponentIDs, entityID);
            }
        }


        public void OnEntityRemoved(MutableList<CompTypeIDPair> components, int entityID)
        {
            removeEntityIfItHadSelectedComponents(components, entityID);
        }

        private void removeEntityIfItHadSelectedComponents(MutableList<CompTypeIDPair> components, int entityID)
        {
            if (componentSelection.findComponentIDs(components, foundComponentIDs))
            {
                OnRemoveRelevantEntity(entityID);
            }
        }

        public void OnAddComponent(MutableList<CompTypeIDPair> components, int entityID, int indexIntoComponentsList)
        {
            int addedComponentTypeID = components[indexIntoComponentsList].ComponentType;
            int index = componentSelection.selectedComponentIndex(addedComponentTypeID);

            bool addedComponentIsASelectedComponent = index == -1;
            if (addedComponentIsASelectedComponent)
                return;

            if (!componentSelection.findComponentIDs(components, foundComponentIDs))
                return;

            OnAddRelevantEntity(foundComponentIDs, entityID);
        }

        public void OnRemoveComponent(MutableList<CompTypeIDPair> components, int entityID, int indexIntoComponentsList)
        {
            removeEntityIfItHadSelectedComponents(components, entityID);
        }

        public abstract void OnAddRelevantEntity(int[] foundComponentIDs, int entityID);
        public abstract void OnRemoveRelevantEntity(int entityID);
    }
}
