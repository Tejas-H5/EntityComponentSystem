using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public struct EntityBuilder
    {
        ECSWorld world;
        int entity_id;
        public int ID => entity_id;

        public EntityBuilder(ECSWorld world, int entity_id)
        {
            this.world = world;
            this.entity_id = entity_id;
        }

        public EntityBuilder With<T>(T component) where T : struct
        {
            world.AddComponent(entity_id, component);
            return this;
        }

    }
}
