using BenchmarkDotNet.Attributes;
using ECS.CustomDataStructures;
using ECSBenchmarking.SceneGraph;
using Common;
using System;
using System.Collections.Generic;
using System.Text;
using ECS;

namespace ECSBenchmarking
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ECSReactiveBenchmark
    {
        [Params(10000)]
        public int NumberOfElements { get; set; }

        const float FRAMERATE = 1f / 60f;

        ECSWorld world = new ECSWorld();

        MotionIntergratorSystem2DReactive motionIntegrator;

        int[] entities;

        [GlobalSetup]
        public void GlobalSetup()
        {
            InitECS();
        }

        private void InitECS()
        {
            motionIntegrator = new MotionIntergratorSystem2DReactive(world);
            entities = world.CreateEntities(NumberOfElements);

            for (int i = 0; i < entities.Length; i++)
            {
                world.AddComponent(entities[i], new Position());
                world.AddComponent(entities[i], new Velocity(0.5f, 0));
                world.AddComponent(entities[i], new Acceleration());
            }
        }

        [Benchmark]
        public void ECSUpdate()
        {
            motionIntegrator.Update(FRAMERATE);
        }
    }
}