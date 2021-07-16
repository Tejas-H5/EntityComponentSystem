using BenchmarkDotNet.Attributes;
using ECS;
using ECS.CustomDataStructures;
using SceneGraph;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECSBenchmarking
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ECSvsSceneGraph
    {
        const float FRAMERATE = 1f / 60f;
        const int NUMELEMENTS = 10000;

        ECSWorld world = new ECSWorld();

        MotionIntergratorSystem2D motionIntegrator;

        uint[] entities;

        SGObject sceenGraphRoot = new SGObject();

        public ECSvsSceneGraph()
        {
            InitNormalECS();
            InitSceneGraph();
        }

        private void InitNormalECS()
        {
            motionIntegrator = new MotionIntergratorSystem2D(world);
            entities = world.CreateEntities(NUMELEMENTS);

            for (int i = 0; i < entities.Length; i++)
            {
                world.AddComponent(entities[i], new Position());
                world.AddComponent(entities[i], new Velocity(0.5f, 0));
                world.AddComponent(entities[i], new Acceleration());
            }
        }

        private void InitSceneGraph()
        {
            for(int i = 0; i < NUMELEMENTS; i++)
            {
                SGObject obj = new SGObject();

                sceenGraphRoot.AddChild(obj);
            }
        }


        [Benchmark]
        public void ECSUpdate()
        {
            motionIntegrator.Update(FRAMERATE);
        }


        [Benchmark]
        public void SceneGraphUpdate()
        {
            sceenGraphRoot.Update(FRAMERATE);
        }
    }
}
