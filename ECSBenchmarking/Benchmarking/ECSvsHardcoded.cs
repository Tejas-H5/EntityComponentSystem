using BenchmarkDotNet.Attributes;
using ECS;
using ECS.CustomDataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECSBenchmarking
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ECSvsHardcodedBenchmarks
    {
        const float FRAMERATE = 1f / 60f;
        const int NUMELEMENTS = 10000;

        ECSWorld world = new ECSWorld();

        MotionIntergratorSystem2D motionIntegrator;

        uint[] entities;

        //This one should be faster, since it has no constraints like the ECS.
        //but How much faster? let us find out
        HardcodedMotionIntergratorSystem2D hardcodedMotionIntegrator;

        MutableList<Acceleration> accels;
        MutableList<Velocity> velocities;
        MutableList<Position> positions;

        public ECSvsHardcodedBenchmarks()
        {
            InitNormalECS();
            InitHardcodedECS();
        }

        private void InitNormalECS()
        {
            motionIntegrator = new MotionIntergratorSystem2D(world);
            entities = world.CreateEntities(NUMELEMENTS);

            for (int i = 0; i < entities.Length; i++)
            {
                world.AddComponent(entities[i], new Position());
                world.AddComponent(entities[i], new Velocity(0.5f,0));
                world.AddComponent(entities[i], new Acceleration());
            }
        }

        private void InitHardcodedECS()
        {
            accels = new MutableList<Acceleration>(NUMELEMENTS);
            for(int i = 0; i < NUMELEMENTS; i++)
            {
                accels.Add(new Acceleration());
            }

            velocities = new MutableList<Velocity>(NUMELEMENTS);
            for (int i = 0; i < NUMELEMENTS; i++)
            {
                velocities.Add(new Velocity(0.5f,0));
            }

            positions = new MutableList<Position>(NUMELEMENTS);
            for (int i = 0; i < NUMELEMENTS; i++)
            {
                positions.Add(new Position());
            }

            hardcodedMotionIntegrator = new HardcodedMotionIntergratorSystem2D(velocities, accels, positions);
        }


        [Benchmark]
        public void ECSUpdate()
        {
            motionIntegrator.Update(FRAMERATE);
        }


        [Benchmark]
        public void HardcodedECSUpdate()
        {
            hardcodedMotionIntegrator.Update(FRAMERATE);
        }
    }
}
