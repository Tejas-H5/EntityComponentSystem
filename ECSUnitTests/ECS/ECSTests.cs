using ECS.ECS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplestECS.ECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestECSUnitTests.ECS
{
    enum Components
    {
        Velocity,
        Position,
        Acceleration
    }

    [ECSComponent((int)Components.Velocity)]
    struct Velocity
    {
        public float X, Y;

        public Velocity(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    [ECSComponent((int)Components.Position)]
    struct Position
    {
        public float X, Y;

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    [ECSComponent((int)Components.Acceleration)]
    struct Acceleration
    {
        public float X, Y;

        public Acceleration(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    class MotionIntergratorSystem2D : ECSSystem
    {
        public MotionIntergratorSystem2D(ECSWorld world) : base(world) {}

        protected override void InitSystem(ECSWorld world)
        {
            SelectComponentTypes(world,
                typeof(Velocity),
                typeof(Position),
                typeof(Acceleration)
            );
        }

        protected override void Iterate(float deltaTime)
        {
            ref Velocity vel = ref GetComponent<Velocity>(0);
            ref Position pos = ref GetComponent<Position>(1);
            ref Acceleration accel = ref GetComponent<Acceleration>(2);

            float halfDelta = 0.5f * deltaTime;

            //Verlet intergration
            pos.X += vel.X * halfDelta;
            pos.Y += vel.Y * halfDelta;

            vel.X += accel.X * deltaTime;
            vel.Y += accel.Y * deltaTime;

            pos.X += vel.X * halfDelta;
            pos.Y += vel.Y * halfDelta;
        }
    }


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
    }
}
