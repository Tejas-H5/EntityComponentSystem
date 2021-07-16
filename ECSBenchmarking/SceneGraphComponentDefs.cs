using ECSBenchmarking.SceneGraph;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECSBenchmarking
{
    public class VelocityComponent : SGComponent
    {
        public float X, Y;

        public VelocityComponent(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    public class PositionComponent : SGComponent
    {
        public float X, Y;

        public PositionComponent(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
    public class AccelerationComponent : SGComponent
    {
        public float X, Y;

        public AccelerationComponent(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
    public class NameComponent : SGComponent
    {
        public string NameString;

        public NameComponent(string name)
        {
            NameString = name;
        }
    }

    public class MotionScript : SGComponent
    {
        PositionComponent pos;
        VelocityComponent vel;
        AccelerationComponent accel;

        public override void Init()
        {
            pos = attachedObject.GetComponent<PositionComponent>();
            vel = attachedObject.GetComponent<VelocityComponent>();
            accel = attachedObject.GetComponent<AccelerationComponent>();
        }

        public override void Update(float deltaTime)
        {
            float halfDelta = deltaTime * 0.5f;

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
