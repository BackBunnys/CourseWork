using System;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Car
{
    internal class Wheel : Cylinder
    {
        public Wheel()
        {
            Size = new Vector3(0.2f, 0.2f, 0.1f);
            Bounds = LocalBounds with {Size = Size};
            Color = Color4.DarkGray;
        }
    }
}