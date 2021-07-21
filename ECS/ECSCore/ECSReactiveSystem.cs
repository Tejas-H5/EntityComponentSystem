using ECS.CustomDataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public abstract class ECSReactiveSystem : ECSRelevantComponentListner
    {
        MultiList<int> componentIDLists;

        List<int> entityIDs = new List<int>();
        Dictionary<int, int> entityIndexMap = new Dictionary<int, int>();
        
        private int NumEntities {
            get { return componentIDLists.Count; }
        }

        public ECSReactiveSystem(ECSWorld world) : base(world)
        {

        }

        protected override void OnAfterComponentTypesHaveBeenSelected()
        {
            componentIDLists = new MultiList<int>(selectedComponentTypeIDs.Length);
        }

        public override void OnAddRelevantEntity(int[] componentIDs, int entityID)
        {
            int index = NumEntities;

            componentIDLists.Add(componentIDs);

            entityIDs.Add(entityID);
            entityIndexMap[entityID] = index;
        }

        public override void OnRemoveRelevantEntity(int entityID)
        {
            int index = entityID;
            int back = NumEntities - 1;

            componentIDLists.Swap(index, back);
            componentIDLists.RemoveAt(back);
        }

        public void Update(float deltaTime)
        {
            for(int i = 0; i < NumEntities; i++)
            {
                for(int j = 0; j < selectedComponentTypeIDs.Length; j++)
                {
                    foundComponentIDs[j] = componentIDLists[i, j];
                }

                Iterate(deltaTime);
            }
        }

        protected ref T GetComponent<T>(int initializationOrder) where T : struct
        {
            ComponentList<T> components = StaticComponentListCache<T>.Get(world.WorldID);
            return ref components[foundComponentIDs[initializationOrder]].Data;
        }


        protected abstract void Iterate(float deltaTime);

        /// <summary>
        /// This function must call SelectComonentTypes.
        /// </summary>
        protected abstract void InitSystem();
    }
}
