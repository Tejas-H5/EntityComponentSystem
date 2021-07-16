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

            createSeveralEntities(world, 4);

            Assert.IsTrue(world.EntityCount == 4);
        }

        private static List<uint> createSeveralEntities(ECSWorld world, int number)
        {
            List<uint> entities = new List<uint>();
            for (int i = 0; i < number; i++)
            {
                uint entity = world.CreateEntity();
                entities.Add(entity);
            }

            return entities;
        }

        [TestMethod]
        public void ECS_EntityDeletion_ShouldCreateAndDelete4Entities()
        {
            ECSWorld world = new ECSWorld();

            List<uint> entities = createSeveralEntities(world, 4);

            world.DestroyEntity(entities[0]);
            world.DestroyEntity(entities[3]);
            world.DestroyEntity(entities[1]);
            world.DestroyEntity(entities[2]);

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

            List<uint> entities = createSeveralEntities(world, 4);
            addPositionAccelerationVelocityComponents(world, entities);

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

        private static void addPositionAccelerationVelocityComponents(ECSWorld world, List<uint> entities)
        {
            addComponentToEntities(world, entities, new Velocity(1, 0));
            addComponentToEntities(world, entities, new Acceleration(0, 0));
            addComponentToEntities(world, entities, new Position(0, 0));
        }

        private static void addComponentToEntities<T>(ECSWorld world, List<uint> entities, T data) where T : struct
        {
            for (int i = 0; i < entities.Count; i++)
            {
                world.AddComponent(entities[i], data);
            }
        }

        [TestMethod]
        public void ECS_CreateAHundredKEntities()
        {
            testThatECSCanCreateNEntities(100000);
        }

        [TestMethod]
        public void ECS_Create1MillionEntities()
        {
            testThatECSCanCreateNEntities(1000000);
        }

        private static void testThatECSCanCreateNEntities(int n)
        {
            ECSWorld world = new ECSWorld();

            MotionIntergratorSystem2D motionIntegrator = new MotionIntergratorSystem2D(world);

            List<uint> entities = createSeveralEntities(world, n);
            addPositionAccelerationVelocityComponents(world, entities);

            Assert.IsTrue(world.EntityCount == n);
        }


        [TestMethod]
        public void ECS_IterateOverAHundredKEntities()
        {
            ECSWorld world = new ECSWorld();

            MotionIntergratorSystem2D motionIntegrator = new MotionIntergratorSystem2D(world);

            List<uint> entities = createSeveralEntities(world, 100000);
            addPositionAccelerationVelocityComponents(world, entities);

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
