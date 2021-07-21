using ECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisualTests.Systems
{
    public class PointTrailDrawer : ECSSystem
    {
        public PointTrailDrawer(ECSWorld world) : base(world,
                typeof(PointTrail)
            )
        {
        }

        protected override void Iterate(float deltaTime)
        {
            ref PointTrail pointTrail = ref GetComponent<PointTrail>(0);
        }
    }
}
