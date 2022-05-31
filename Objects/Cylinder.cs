using System;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class Cylinder : Figure
    {
        private const int CircleVerticesCount = 32;
        private static readonly Mesh CappedCylinderMesh;
        public static readonly Mesh CylinderMesh;

        static Cylinder()
        {
            double theta = 0;
            const double dt = MathHelper.TwoPi / CircleVerticesCount;

            var canvasPoints = new Vector2[CircleVerticesCount];

            for (var i = 0; i < CircleVerticesCount; ++i)
            {
                var x = (float) Math.Cos(theta);
                var y = (float) Math.Sin(theta);

                canvasPoints[i] = new Vector2(x, y);
                theta += dt;
            }

            CappedCylinderMesh = new Mesh(GeometryHelper.CalculateNewellNormals(GeometryHelper.Extrude(canvasPoints)));
            CylinderMesh = new Mesh(GeometryHelper.CalculateNewellNormals(GeometryHelper.Extrude(canvasPoints, false)));
        }


        public Cylinder(bool capped = true) : base(capped ? CappedCylinderMesh : CylinderMesh)
        {
        }
    }
}