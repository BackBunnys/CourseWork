using System.Collections.Generic;
using System.Linq;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class ErmitQurve : Figure
    {
        private static readonly Matrix4 M;

        static ErmitQurve()
        {
            M = new Matrix4(new Vector4(1, 0, -3, 2),
                new Vector4(0, 0, 3, -2), new Vector4(0, 1, -2, 1),
                new Vector4(0, 0, -1, 1));
        }

        public static IEnumerable<Vector2> CalculatePoints(IList<KeyValuePair<Vector3, Vector3>> pointsVectors, uint sectorPointsCount)
        {
            var points = new List<Vector2>((int) (pointsVectors.Count * sectorPointsCount));

            for (var i = 0; i < pointsVectors.Count - 1; ++i)
            {
                var current = pointsVectors[i];
                var next = pointsVectors[i + 1];
                points.AddRange(CalculatePoints(current.Key, current.Value, next.Key, next.Value, sectorPointsCount));
            }

            return points;
        }

        public static IEnumerable<Vector2> CalculatePoints(Vector3 p1, Vector3 q1, Vector3 p2, Vector3 q2,
            uint pointsCount)
        {
            var temp = new Matrix4x3(p1, p2, q1, q2);
            var G = new Matrix4(temp.Column0, temp.Column1, temp.Column2, Vector4.Zero);

            var points = new Vector2[pointsCount];

            for (var i = 0; i < pointsCount; ++i)
            {
                var t = (float) i / (pointsCount - 1);
                var T = new Vector4(1, t, t * t, t * t * t);

                points[i] = (G * M * T).Xy;
            }

            return points;
        }

        private static Mesh CreateMesh(Vector3 p1, Vector3 q1, Vector3 p2, Vector3 q2, uint pointsCount)
        {
            var points = CalculatePoints(p1, q1, p2, q2, pointsCount);
            var indices = new uint[pointsCount];

            for (var i = 0; i < pointsCount; ++i)
            {
                indices[i] = (uint) i;
            }

            var mesh = new Mesh(points.Select(p => new Vertex(new Vector3(p.X, p.Y, 0))), indices);
            mesh.Vao.Mode = PrimitiveType.LineStrip;

            return mesh;
        }

        public ErmitQurve(Vector3 p1, Vector3 q1, Vector3 p2, Vector3 q2, uint pointsCount = 50) : base(CreateMesh(p1,
            q1, p2, q2, pointsCount))
        {
        }
    }
}