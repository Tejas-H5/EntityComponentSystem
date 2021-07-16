using ECS;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace VisualTests
{
    [ECSComponent((int)ComponentEnum.Motion)]
    public struct Motion
    {
        public Vector3 Acceleration;
        public Vector3 Velocity;
        public Vector3 RotaionAngle;
        public float RotationVelocity;

        public Motion(Vector3 acceleration = default, Vector3 velocity = default, Vector3 rotaionAngle = default, float rotationVelocity = 0)
        {
            this.Acceleration = acceleration;
            this.Velocity = velocity;
            this.RotaionAngle = rotaionAngle;
            this.RotationVelocity = rotationVelocity;
        }
    }
}
