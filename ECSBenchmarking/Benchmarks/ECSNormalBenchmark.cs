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
    public class ECSNormalBenchmark
    {
        [Params(10000)]
        public int NumberOfElements { get; set; }

        const float FRAMERATE = 1f / 60f;

        ECSWorld world = new ECSWorld();

        MotionIntergratorSystem2D motionIntegrator;

        int[] entities;

        [GlobalSetup]
        public void GlobalSetup()
        {
            InitECS();
        }

        private void InitECS()
        {
            motionIntegrator = new MotionIntergratorSystem2D(world);
            entities = new int[NumberOfElements];

            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = world.CreateEntity()
                    .With(new Position())
                    .With(new Velocity(0.5f, 0))
                    .With(new Acceleration())
                    .ID;
            }
        }

        [Benchmark]
        public void ECSUpdate()
        {
            motionIntegrator.Update(FRAMERATE);
        }
    }
}
