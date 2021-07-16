using ECS;
using ECSUnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestECSUnitTests.ECS
{
    [TestClass]
    public class ECSTests
    {
        [TestMethod]
        public void ECS_WorldCreation_ShouldntThrowExceptions()
        {
            ECSWorld world = new ECSWorld();
        }


        [TestMethod]
        public void ECS_EntityCreation_ShouldCreate4Entities()
        {
            ECSWorld world = new ECSWorld();

            uint entity = world.CreateEntity();
            uint entity2 = world.CreateEntity();
            uint entity3 = world.CreateEntity();
            uint entity4 = world.CreateEntity();

            Assert.IsTrue(world.EntityCount == 4);
        }

        [TestMethod]
        public void ECS_EntityDeletion_ShouldCreateAndDelete4Entities()
        {
            ECSWorld world = new ECSWorld();

            uint entity = world.CreateEntity();
            uint entity2 = world.CreateEntity();
            uint entity3 = world.CreateEntity();
            uint entity4 = world.CreateEntity();

            world.DestroyEntity(entity);
            world.DestroyEntity(entity4);
            world.DestroyEntity(entity2);
            world.DestroyEntity(entity3);

            Assert.IsTrue(world.EntityCount == 0);
        }

        [TestMethod]
        public void ECS_SuperBasicDemo_ShouldWork()
        {
            ECSWorld world = new ECSWorld();

            MotionIntergratorSystem2D motionIntegrator = new MotionIntergratorSystem2D(world);

            uint entity = world.CreateEntity();
            world.AddComponent(entity, new Position(0, 0));
            world.AddComponent(entity, new Velocity(1, 0));
            world.AddComponent(entity, new Acceleration(0, 0));

            float framerate = 1f / 60f;
            for (float t = 0; t < 10f; t += framerate)
            {
                motionIntegrator.Update(framerate);
            }

            Position pos = world.GetComponentFromEntity<Position>(entity);
            Assert.IsTrue(pos.X > 9);
            Assert.IsTrue(pos.X < 11);
        }


        [TestMethod]
        public void ECS_IterateOverMultipleEntities()
        {
            ECSWorld world = new ECSWorld();

            MotionIntergratorSystem2D motionIntegrator = new MotionIntergratorSystem2D(world);

            List<uint> entities = new List<uint>();
            for(int i = 0; i < 4; i++)
            {
                uint entity = world.CreateEntity();
                world.AddComponent(entity, new Position(0, 0));
                world.AddComponent(entity, new Velocity(1, 0));
                world.AddComponent(entity, new Acceleration(0, 0));
                entities.Add(entity);
            }
            

            float framerate = 1f / 60f;
            for (float t = 0; t < 10f; t += framerate)
            {
                motionIntegrator.Update(framerate);
            }

            List<Position> positions = new List<Position>();

            for (int i = 0; i < entities.Count; i++)
            {
                Position pos = world.GetComponentFromEntity<Position>(entities[i]);
                positions.Add(pos);
            }

            for (int i = 0; i < entities.Count; i++)
            {
                Position pos = world.GetComponentFromEntity<Position>(entities[i]);
                Assert.IsTrue(pos.X > 9);
                Assert.IsTrue(pos.X < 11);
            }
        }


        [TestMethod]
        public void ECS_CreateAHundredKEntities()
        {
            ECSWorld world = new ECSWorld();

            MotionIntergratorSystem2D motionIntegrator = new MotionIntergratorSystem2D(world);

            List<uint> entities = new List<uint>();
            for (int i = 0; i < 100000; i++)
            {
                uint entity = world.CreateEntity();
                world.AddComponent(entity, new Position(0, 0));
                world.AddComponent(entity, new Velocity(1, 0));
                world.AddComponent(entity, new Acceleration(0, 0));
                entities.Add(entity);
            }
        }

        [TestMethod]
        public void ECS_Create1MillionEntities()
        {
            ECSWorld world = new ECSWorld();

            MotionIntergratorSystem2D motionIntegrator = new MotionIntergratorSystem2D(world);

            List<uint> entities = new List<uint>();
            for (int i = 0; i < 1000000; i++)
            {
                uint entity = world.CreateEntity();
                world.AddComponent(entity, new Position(0, 0));
                world.AddComponent(entity, new Velocity(1, 0));
                world.AddComponent(entity, new Acceleration(0, 0));
                entities.Add(entity);
            }
        }


        [TestMethod]
        public void ECS_IterateOverAHundredKEntities()
        {
            ECSWorld world = new ECSWorld();

            MotionIntergratorSystem2D motionIntegrator = new MotionIntergratorSystem2D(world);

            List<uint> entities = new List<uint>();
            for (int i = 0; i < 100000; i++)
            {
                uint entity = world.CreateEntity();
                world.AddComponent(entity, new Position(0, 0));
                world.AddComponent(entity, new Velocity(1, 0));
                world.AddComponent(entity, new Acceleration(0, 0));
                entities.Add(entity);
            }


            float framerate = 1f / 60f;
            for (float t = 0; t < 10f; t += framerate)
            {
                motionIntegrator.Update(framerate);
            }

            for (int i = 0; i < entities.Count; i++)
            {
                Position pos = world.GetComponentFromEntity<Position>(entities[i]);
                Assert.IsTrue(pos.X > 9);
                Assert.IsTrue(pos.X < 11);
            }
        }

        /* TODO - Add the following tests:
         * Iterate over multiple entities
         * Deleting components
         *   - makeing sure that the 
         * 
         * 
         * 
         * 
         */
    }
}
