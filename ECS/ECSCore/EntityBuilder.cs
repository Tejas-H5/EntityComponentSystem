using System;
using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public class EntityBuilder
    {
        ECSWorld world;
        public EntityBuilder(ECSWorld world)
        {
            this.world = world;
        }

        

        public void With<T>(T component) where T : struct
        {

        }
    }
}
