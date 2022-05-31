using System;
using System.Collections.Generic;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Car
{
    public class Body : Figure
    {
        private static readonly Mesh BodyMesh;

        static Body()
        {
            var geometryData = GeometryHelper.Extrude(
                new List<Vector2>()
                {
                    new(0.25f, 0),
                    new(0.5f, 0),
                    new(0.75f, 0),
                    new(1, 0),
                    new(1, 0.2f),
                    new(0.6f, 0.2f),
                    new(0.5f, 0.4f),
                    new(-0.2f, 0.4f),
                    new(-0.3f, 0.2f),
                    new(-0.9f, 0.2f),
                    new(-1, 0.12f),
                    new(-1.04f, 0.08f),
                    new(-1.04f, 0.04f),
                    new(-1, 0),
                });

            BodyMesh = new Mesh(GeometryHelper.CalculateNewellNormals(geometryData));
        }

        public Body() : base(BodyMesh)
        {
            Color = Color4.Blue;
        }
    }
}