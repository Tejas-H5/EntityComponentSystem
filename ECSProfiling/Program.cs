using System;
using System.Collections.Generic;
using ECS;
using Common;
using System.Diagnostics;

namespace ECSProfiling
{
    class Program
    {
        static void Main(string[] args)
        {
            Debugger.Launch();

            ECSWorld world = new ECSWorld();

            ECSSystem motionIntegrator = new MotionIntergratorSystem2D(world);

            List<int> entities = createSeveralEntities(world, 100000);

            float framerate = 1f / 60f;
            //for (float t = 0; t < 10f; t += framerate)
            while(true)
            {
                motionIntegrator.Update(framerate);
            }
        }

        private static List<int> createSeveralEntities(ECSWorld world, int v)
        {
            List<int> entities = new List<int>();
            entities.Capacity = v;
            for(int i = 0; i < v; i++)
            {
                entities.Add(
                    world.CreateEntity()
                        .With(new Acceleration(0, 0))
                        .With(new Position(0, 0))
                        .With(new Velocity(1, 0))
                        .ID
                    );
            }

            return entities;
        }
    }
}
