using BenchmarkDotNet.Attributes;
using ECS;
using ECS.CustomDataStructures;
using ECSBenchmarking.SceneGraph;
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECSBenchmarking
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ECSvsOthersBenchmarks
    {
        [Params(100000)]
        public int NumberOfElements { get; set; }


        const float FRAMERATE = 1f / 60f;

        ECSWorld world = new ECSWorld();

        MotionIntergratorSystem2D motionIntegrator;

        int[] entities;

        SGObject sceenGraphRoot = new SGObject();

        //This one should be faster, since it has no constraints like the ECS.
        //but How much faster? let us find out
        HardcodedMotionIntergratorSystem2D hardcodedMotionIntegrator;

        MutableList<Acceleration> accels;
        MutableList<Velocity> velocities;
        MutableList<Position> positions;

        [GlobalSetup]
        public void GlobalSetup()
        {
            InitNormalECS();
            InitHardcodedECS();
            InitSceneGraph();
        }

        private void InitNormalECS()
        {
            motionIntegrator = new MotionIntergratorSystem2D(world);
            entities = world.CreateEntities(NumberOfElements);

            for (int i = 0; i < entities.Length; i++)
            {
                world.AddComponent(entities[i], new Position());
                world.AddComponent(entities[i], new Velocity(0.5f,0));
                world.AddComponent(entities[i], new Acceleration());
            }
        }

        private void InitHardcodedECS()
        {
            accels = new MutableList<Acceleration>(NumberOfElements);
            for(int i = 0; i < NumberOfElements; i++)
            {
                accels.Add(new Acceleration());
            }

            velocities = new MutableList<Velocity>(NumberOfElements);
            for (int i = 0; i < NumberOfElements; i++)
            {
                velocities.Add(new Velocity(0.5f,0));
            }

            positions = new MutableList<Position>(NumberOfElements);
            for (int i = 0; i < NumberOfElements; i++)
            {
                positions.Add(new Position());
            }

            hardcodedMotionIntegrator = new HardcodedMotionIntergratorSystem2D(velocities, accels, positions);
        }
        private void InitSceneGraph()
        {
            for (int i = 0; i < NumberOfElements; i++)
            {
                SGObject obj = new SGObject();

                obj.AddComponent(new PositionComponent(0, 0));
                obj.AddComponent(new VelocityComponent(0.5f, 0));
                obj.AddComponent(new AccelerationComponent(0, 0));

                sceenGraphRoot.AddChild(obj);
            }
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

        [Benchmark]
        public void SceneGraphUpdate()
        {
            sceenGraphRoot.Update(FRAMERATE);
        }
    }
}
