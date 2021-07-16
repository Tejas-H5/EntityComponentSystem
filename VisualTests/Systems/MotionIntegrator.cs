using ECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisualTests.Systems
{
    public class MotionIntegrator : ECSSystem
    {
        public MotionIntegrator(ECSWorld world) : base(world)
        {
        }

        protected override void InitSystem()
        {
            SelectComponentTypes(
                    typeof(Transform),
                    typeof(Motion)
                );
        }

        protected override void Iterate(float deltaTime)
        {
            ref Transform transform = ref GetComponent<Transform>(0);
            ref Motion motion = ref GetComponent<Motion>(1);

            Verlet(deltaTime, ref transform, ref motion);

            //TODO: angular momentum
        }

        private static void Verlet(float deltaTime, ref Transform transform, ref Motion motion)
        {
            float halfDelta = deltaTime * 0.5f;

            transform.Position += halfDelta * motion.Velocity;

            motion.Velocity += deltaTime * motion.Acceleration;

            transform.Position += halfDelta * motion.Velocity;
        }
    }
}
