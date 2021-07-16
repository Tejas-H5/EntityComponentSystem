using ECS;
using ECS.CustomDataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECSBenchmarking
{
    enum Components
    {
        Velocity,
        Position,
        Acceleration,
        Name
    }

    [ECSComponent((int)Components.Velocity)]
    public struct Velocity
    {
        public float X, Y;

        public Velocity(float x, float y)
        {
            X = x;
            Y = y;
        }
    }



    [ECSComponent((int)Components.Position)]
    public struct Position
    {
        public float X, Y;

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }
    }




    [ECSComponent((int)Components.Acceleration)]
    public struct Acceleration
    {
        public float X, Y;

        public Acceleration(float x, float y)
        {
            X = x;
            Y = y;
        }
    }



    [ECSComponent((int)Components.Name)]
    public struct Name
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

        protected override void InitSystem()
        {
            SelectComponentTypes(
                typeof(Velocity),
                typeof(Position),
                typeof(Acceleration)
            );
        }

        protected override void Iterate(float deltaTime)
        {
            Velocity vel = GetComponent<Velocity>(0);
            Position pos = GetComponent<Position>(1);
            Acceleration accel = GetComponent<Acceleration>(2);

            float halfDelta = 0.5f * deltaTime;

            //Verlet intergration
            pos.X += vel.X * halfDelta;
            pos.Y += vel.Y * halfDelta;

            vel.X += accel.X * deltaTime;
            vel.Y += accel.Y * deltaTime;

            pos.X += vel.X * halfDelta;
            pos.Y += vel.Y * halfDelta;

            GetComponent<Velocity>(0) = vel;
            GetComponent<Position>(1) = pos;
            GetComponent<Acceleration>(2) = accel;
        }
    }

    class HardcodedMotionIntergratorSystem2D
    {
        MutableList<Velocity> velocities;
        MutableList<Acceleration> accelerations;
        MutableList<Position> positions;

        public HardcodedMotionIntergratorSystem2D(MutableList<Velocity> velocities, 
            MutableList<Acceleration> accelerations, 
            MutableList<Position> positions
            )
        {
            this.velocities = velocities;
            this.accelerations = accelerations;
            this.positions = positions;
        }

        public void Update(float deltaTime)
        {
            for(int i = 0; i < positions.Count; i++)
            {
                Iterate(deltaTime,
                        ref velocities[i],
                        ref accelerations[i],
                        ref positions[i]
                    );
            }
        }

        void Iterate(float deltaTime, ref Velocity vel, ref Acceleration accel, ref Position pos)
        {
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
