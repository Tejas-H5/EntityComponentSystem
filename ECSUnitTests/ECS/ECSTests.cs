using Common;
using ECS;
using ECSUnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECSUnitTests.ECS
{
    [TestClass]
    public abstract class ECSTests
    {
        protected abstract IECSSystem CreateSystem(ECSWorld world);

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

        private static void breakPoint() { }

        private static List<int> createSeveralEntities(ECSWorld world, int number)
        {
            List<int> entities = new List<int>();
            for (int i = 0; i < number; i++)
            {
                int entity = world.CreateEntity().ID;
                entities.Add(entity);
            }

            return entities;
        }

        [TestMethod]
        public void ECS_EntityDeletion_ShouldCreateAndDelete4Entities()
        {
            ECSWorld world = new ECSWorld();

            List<int> entities = createSeveralEntities(world, 4);

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

            IECSSystem motionIntegrator = CreateSystem(world);

            int entity = world.CreateEntity()
                .With(new Position(0, 0))
                .With(new Velocity(1, 0))
                .With(new Acceleration(0, 0))
                .ID;

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

            IECSSystem motionIntegrator = CreateSystem(world);

            List<int> entities = createSeveralEntities(world, 100000);
            addPositionAccelerationVelocityComponents(world, entities);

            float framerate = 1f / 60f;
            for (float t = 0; t < 10f; t += framerate)
            {
                motionIntegrator.Update(framerate);
            }

            assertPos_XIsAt(world, entities, 10, 1);
        }
        private static void assertPos_XIsAt(ECSWorld world, List<int> entities, float value, float tolerance)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (!world.HasComponent<Position>(entities[i]))
                    continue;

                Position pos = world.GetComponentFromEntity<Position>(entities[i]);

                Assert.IsTrue(MathF.Abs(pos.X - value) < tolerance);
            }
        }


        [TestMethod]
        public void ECS_System_IgnoreIrellevantEntitiesWhenIterating()
        {
            ECSWorld world = new ECSWorld();

            IECSSystem motionIntegrator = CreateSystem(world);

            List<int> entities = createSeveralEntities(world, 1000000);

            List<int> subset = new List<int>
            {
                entities[0],
                entities[1],
                entities[2],
                entities[3],
            };

            addPositionAccelerationVelocityComponents(world, subset);
            for(int i = 0; i < entities.Count; i++)
            {
                world.AddComponent(entities[i], new Name("Entity " + i));
            }


            DateTime startTime = DateTime.Now;

            float framerate = 1f / 60f;
            for (float t = 0; t < 10f; t += framerate)
            {
                motionIntegrator.Update(framerate);
            }

            double timeTaken = (DateTime.Now - startTime).TotalMilliseconds;

            Assert.IsTrue(timeTaken < 10);

            assertPos_XIsAt(world, subset, 10, 1);
        }

        private static void addPositionAccelerationVelocityComponents(ECSWorld world, List<int> entities)
        {
            addComponentToEntities(world, entities, new Velocity(1, 0));
            addComponentToEntities(world, entities, new Acceleration(0, 0));
            addComponentToEntities(world, entities, new Position(0, 0));
        }

        private static void addComponentToEntities<T>(ECSWorld world, List<int> entities, T data) where T : struct
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

        private void testThatECSCanCreateNEntities(int n)
        {
            ECSWorld world = new ECSWorld();

            IECSSystem motionIntegrator = CreateSystem(world);

            List<int> entities = createSeveralEntities(world, n);
            addPositionAccelerationVelocityComponents(world, entities);

            Assert.IsTrue(world.EntityCount == n);
        }


        [TestMethod]
        public void ECS_IterateOverMultipleEntitiesAfterDeletionFromFront()
        {
            ECSWorld world = new ECSWorld();

            IECSSystem motionIntegrator = CreateSystem(world);

            List<int> entities = createSeveralEntities(world, 10);
            addPositionAccelerationVelocityComponents(world, entities);

            world.DestroyEntity(entities[0]);
            world.DestroyEntity(entities[1]);
            world.DestroyEntity(entities[2]);
            world.DestroyEntity(entities[3]);

            float framerate = 1f / 60f;
            for (float t = 0; t < 10f; t += framerate)
            {
                motionIntegrator.Update(framerate);
            }

            assertPos_XIsAt(world, entities, 10, 1);
        }

        [TestMethod]
        public void ECS_IterateOverMultipleEntitiesAfterDeletionFromBack()
        {
            ECSWorld world = new ECSWorld();

            IECSSystem motionIntegrator = CreateSystem(world);

            List<int> entities = createSeveralEntities(world, 10);
            addPositionAccelerationVelocityComponents(world, entities);

            world.DestroyEntity(entities[entities.Count - 1]);
            world.DestroyEntity(entities[entities.Count - 2]);
            world.DestroyEntity(entities[entities.Count - 3]);
            world.DestroyEntity(entities[entities.Count - 4]);

            float framerate = 1f / 60f;
            for (float t = 0; t < 10f; t += framerate)
            {
                motionIntegrator.Update(framerate);
            }

            assertPos_XIsAt(world, entities, 10, 1);
        }


        [TestMethod]
        public void ECS_ComponentsStayOnSameEntityAfterOthersAreDeleted()
        {
            ECSWorld world = new ECSWorld();

            IECSSystem motionIntegrator = CreateSystem(world);

            List<int> entities = createSeveralEntities(world, 10);
            addPositionAccelerationVelocityComponents(world, entities);
            
            world.AddComponent(entities[entities.Count - 1], new Name("Usain"));
            world.GetComponentFromEntity<Velocity>(entities[entities.Count - 1]).X = 2;

            world.DestroyEntity(entities[0]);
            world.DestroyEntity(entities[1]);
            world.DestroyEntity(entities[2]);
            world.DestroyEntity(entities[3]);

            float framerate = 1f / 60f;
            for (float t = 0; t < 10f; t += framerate)
            {
                motionIntegrator.Update(framerate);
            }

            for (int i = 0; i < entities.Count; i++)
            {
                if (!world.HasComponent<Position>(entities[i]))
                    continue;

                Position pos = world.GetComponentFromEntity<Position>(entities[i]);

                if(pos.X > 19 && pos.X < 21)
                {
                    Assert.IsTrue(world.GetComponentFromEntity<Name>(entities[entities.Count - 1]).NameString().Equals("Usain"));
                }
                else
                {
                    Assert.IsTrue(pos.X > 9);
                    Assert.IsTrue(pos.X < 11);
                }
            }
        }

        /* TODO - Add the following tests:
         * Removeing components
         * Ensure that we are actually iterating through the shortest list of the selected components
         * 
         * 
         * 
         */
    }
}
