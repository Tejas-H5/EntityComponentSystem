using ECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECSUnitTests
{
    enum Components
    {
        Velocity,
        Position,
        Acceleration,
        Name
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

    [ECSComponent((int)Components.Name)]
    struct Name
    {
        public string NameString;

        public Name(string name)
        {
            NameString = name;
        }
    }


    class MotionIntergratorSystem2D : ECSSystem
    {
        public MotionIntergratorSystem2D(ECSWorld world) : base(world) { }

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

}
