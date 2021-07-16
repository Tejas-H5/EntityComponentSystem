using ECS;
using MinimalAF.Datatypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisualTests
{
    [ECSComponent((int)ComponentEnum.PointTrail)]
    public struct PointTrail
    {
        public float Radius;
        public Color4 Color;

        public PointTrail(float radius, Color4 color)
        {
            Radius = radius;
            Color = color;
        }
    }
}
