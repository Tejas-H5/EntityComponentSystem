using ECS.CustomDataStructures;
using System;
using System.Collections.Generic;

namespace ECS
{
    public class ECSFilteredEntityList : ECSRelevantComponentListner
    {
        MultiList<int> componentIDLists;
        Dictionary<int, int> entityIndexMap = new Dictionary<int, int>();

        public int NumEntities {
            get { return componentIDLists.Length; }
        }

        public ECSWorld World {
            get {
                return world;
            }
        }

        public ECSFilteredEntityList(ECSWorld world, ComponentSelection selection) : base(world, selection)
        {
            componentIDLists = new MultiList<int>(componentSelection.Length);
        }

        public override void OnAddRelevantEntity(int[] componentIDs, int entityID)
        {
            //order matters
            int index = NumEntities;

            componentIDLists.Add(componentIDs);

            entityIndexMap[entityID] = index;
        }


        public override void OnRemoveRelevantEntity(int entityID)
        {
            int index = entityIndexMap[entityID];
            int back = NumEntities - 1;

            componentIDLists.Swap(index, back);
            componentIDLists.RemoveAt(back);

            entityIndexMap.Remove(entityID);
        }

        public int GetComponentID(int entity, int selectedComponentIndex)
        {
            return componentIDLists[entity, selectedComponentIndex];
        }
    }
}
