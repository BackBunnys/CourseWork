using System.Collections.Generic;
using CourseWork.Common;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Environment
{
    class FlatTerrain : Figure
    {
        private static Mesh CreateMesh(Vector2 size)
        {
            var vertices = new Vertex[(int) (size.X * size.Y)];
            var indices = new List<uint>((int) (size.X * size.Y * 3));
            for (var i = 0; i < size.Y; ++i)
            {
                for (var j = 0; j < size.X; ++j)
                    vertices[(int) (i * size.X + j)] = new Vertex(new Vector3(j - size.X / 2, 0, i - size.Y / 2), new Vector2(i, j),
                        new Vector3(0, 1, 0));
            }

            for (var i = 0; i < size.Y - 1; ++i)
            {
                for (var j = 0; j < size.X - 1; ++j)
                    indices.AddRange(GeometryHelper.Triangulate(new[]
                    {
                        (uint) (i * size.X + j), (uint) (i * size.X + j + 1), (uint) ((i + 1) * size.X + j + 1),
                        (uint) ((i + 1) * size.X + j)
                    }));
            }

            return new Mesh(new GeometryData(vertices, indices));
        }

        public FlatTerrain(Vector2 size) : base(CreateMesh(size))
        {
        }
    }
}