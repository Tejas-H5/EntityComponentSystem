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
        protected int[] selectedComponentTypeIDs;

        public ECSRelevantComponentListner(ECSWorld world) {
            this.world = world;
        }

        protected void SelectComponentTypes(params Type[] types)
        {
            selectedComponentTypeIDs = new int[types.Length];
            foundComponentIDs = new int[types.Length];

            for (int i = 0; i < types.Length; i++)
            {
                selectComponentType(types, i);
            }

            OnAfterComponentTypesHaveBeenSelected();
        }

        private void selectComponentType(Type[] types, int i)
        {
            int typeID = world.GetTypeID(types[i]);
            selectedComponentTypeIDs[i] = typeID;
        }


        public void OnEntityCreated(MutableList<CompTypeIDPair> components, int entityID)
        {
            addEntityIfItHasSelectedComponents(components, entityID);
        }

        private void addEntityIfItHasSelectedComponents(MutableList<CompTypeIDPair> components, int entityID)
        {
            if (findSelectedTypes(components))
            {
                OnAddRelevantEntity(foundComponentIDs, entityID);
            }
        }

        private bool findSelectedTypes(MutableList<CompTypeIDPair> components)
        {
            for (int i = 0; i < selectedComponentTypeIDs.Length; i++)
            {
                int selType = selectedComponentTypeIDs[i];
                bool selectedComponentWasFound = false;

                for (int j = 0; j < components.Count; j++)
                {
                    if (components[j].ComponentType == selType)
                    {
                        selectedComponentWasFound = true;
                        foundComponentIDs[i] = components[j].ComponentID;
                        break;
                    }
                }

                if (!selectedComponentWasFound)
                    return false;
            }

            return true;
        }

        public void OnEntityRemoved(MutableList<CompTypeIDPair> components, int entityID)
        {
            removeEntityIfItHadSelectedComponents(components, entityID);
        }

        private void removeEntityIfItHadSelectedComponents(MutableList<CompTypeIDPair> components, int entityID)
        {
            if (findSelectedTypes(components))
            {
                OnRemoveRelevantEntity(entityID);
            }
        }

        public void OnAddComponent(MutableList<CompTypeIDPair> components, int entityID, int indexIntoComponentsList)
        {
            int addedComponentTypeID = components[indexIntoComponentsList].ComponentType;
            int index = selectedComponentIndex(addedComponentTypeID);

            bool addedComponentIsASelectedComponent = index == -1;
            if (addedComponentIsASelectedComponent)
                return;

            if (!findSelectedTypes(components))
                return;

            OnAddRelevantEntity(foundComponentIDs, entityID);
        }

        private int selectedComponentIndex(int type)
        {
            for (int i = 0; i < selectedComponentTypeIDs.Length; i++)
            {
                if (selectedComponentTypeIDs[i] == type)
                {
                    return i;
                }
            }
            return -1;
        }

        public void OnRemoveComponent(MutableList<CompTypeIDPair> components, int entityID, int indexIntoComponentsList)
        {
            removeEntityIfItHadSelectedComponents(components, entityID);
        }

        protected abstract void OnAfterComponentTypesHaveBeenSelected();
        public abstract void OnAddRelevantEntity(int[] foundComponentIDs, int entityID);
        public abstract void OnRemoveRelevantEntity(int entityID);
    }
}
