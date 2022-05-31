using System;
using System.Collections.Generic;
using System.Linq;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class RotationBody : Figure
    {
        public RotationBody(IEnumerable<Vector2> generatrix, uint sides, Vector3 rotateAround) : base(
            CreateMesh(generatrix, sides, rotateAround))
        {
        }

        public RotationBody(IEnumerable<Vector2> generatrix, uint sides) : base(CreateMesh(generatrix, sides,
            Vector3.UnitY))
        {
        }

        private static Mesh CreateMesh(IEnumerable<Vector2> generatrix, uint sides, Vector3 rotateAround)
        {
            var enumerable = generatrix as Vector2[] ?? generatrix.ToArray();
            var pointsCount = (uint) enumerable.Length;

            var vertices = new List<Vertex>((int) (enumerable.Length * sides));
            var indices = new List<uint>();

            var deltaAngle = 360f / sides;

            for (var angle = 0f; angle <= 360f; angle += deltaAngle)
            {
                vertices.AddRange(enumerable.Select((p, index) =>
                {
                    var rotationMatrix = GeometryHelper.CreateRotationMatrix(rotateAround * angle);

                    var s = angle / 360f;
                    var t = index / (float) pointsCount;
                    return new Vertex((new Vector4(new Vector3(p)) * rotationMatrix).Xyz, new Vector2(s, t));
                }));
            }

            // indices
            //  k1--k1+1
            //  |  / |
            //  | /  |
            //  k2--k2+1
            for (var i = 0u; i < sides; ++i)
            {
                var k1 = i * pointsCount;
                var k2 = k1 + pointsCount;

                for (var j = 0u; j < pointsCount - 1; ++j, ++k1, ++k2)
                {
                    indices.AddRange(new[] {k1, k2, k1 + 1});
                    indices.AddRange(new[] {k1 + 1, k2, k2 + 1});
                }
            }

            var tempIndices = new List<uint>(indices);
            indices.Reverse();
            tempIndices.AddRange(indices.Select(index => (uint)(index + vertices.Count)));
            indices = tempIndices;
            tempIndices.AddRange(indices);
            indices = tempIndices;

            var tempVertices = new List<Vertex>(vertices);
            tempVertices.AddRange(vertices);
            vertices = tempVertices;

            return new Mesh(GeometryHelper.CalculateNewellNormals(new GeometryData(vertices, indices)));
        }
    }
}