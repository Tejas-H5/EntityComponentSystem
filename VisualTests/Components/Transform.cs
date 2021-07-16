using ECS;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace VisualTests
{
    [ECSComponent((int)ComponentEnum.Transform)]
    public struct Transform
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
        }

        public Matrix4x4 ToMatrix()
        {
            return Matrix4x4.CreateTranslation(Position) * Matrix4x4.CreateFromQuaternion(Rotation) * Matrix4x4.CreateScale(Scale);
        }
    }
}
