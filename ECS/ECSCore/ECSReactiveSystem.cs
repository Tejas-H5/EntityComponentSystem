using System;
using System.Text;

namespace ECS
{
    public abstract class ECSReactiveSystem : IECSSystem
    {
        private readonly ECSFilteredEntityList filteredEntityList;
        private readonly int[] foundComponentIDs;

        private readonly ECSWorld world;
        private readonly ComponentSelection componentSelection;

        /// <summary>
        /// Mainly used in the Iterate() function to get the selected components.
        /// </summary>
        protected ref T GetComponent<T>(int initializationOrder) where T : struct
        {
            ComponentList<T> components = StaticComponentListCache<T>.Get(world.WorldID);
            int componentID = foundComponentIDs[initializationOrder];
            return ref components[componentID].Data;
        }

        public ECSReactiveSystem(ECSWorld world, params Type[] types)
        {
            this.world = world;
            componentSelection = new ComponentSelection(this.world, types);

            filteredEntityList = new ECSFilteredEntityList(world, componentSelection);
            foundComponentIDs = new int[componentSelection.Length];
            world.SubscribeListener(filteredEntityList);
        }

        public void Update(float deltaTime)
        {
            for(int i = 0; i < filteredEntityList.NumEntities; i++)
            {
                for(int j = 0; j < componentSelection.Length; j++)
                {
                    foundComponentIDs[j] = filteredEntityList.GetComponentID(i, j);
                }

                Iterate(deltaTime);
            }
        }


        protected abstract void Iterate(float deltaTime);
    }
}
