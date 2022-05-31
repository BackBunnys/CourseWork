using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class Box : Figure
    {
        public static readonly Mesh BoxMesh;

        private static readonly Vertex[] Vertices =
        {
            new(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0, 1)),
            new(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 0)),
            new(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 1)),
            new(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1, 0)),

            new(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0, 1)),
            new(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 0)),
            new(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 1)),
            new(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1, 0)),

            new(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 1)),
            new(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 0)),
            new(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(1, 1)),
            new(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(1, 0)),

            new(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(0, 1)),
            new(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(0, 0)),
            new(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 1)),
            new(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 0)),

            new(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 1)),
            new(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 0)),
            new(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1, 1)),
            new(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1, 0)),

            new(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0, 1)),
            new(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0, 0)),
            new(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 1)),
            new(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 0)),
        };


        private static readonly uint[] Indices =
        {
            0, 2, 1, // front
            1, 2, 3,

            5, 6, 4, // back
            7, 6, 5,

            8, 10, 9, //top
            9, 10, 11,

            13, 14, 12, //bottom
            15, 14, 13,

            17, 18, 16,// left
            19, 18, 17,

            20, 22, 21,//right
            21, 22, 23
        };

        static Box()
        {
            BoxMesh = new Mesh(GeometryHelper.CalculateNormals(new GeometryData(Vertices, Indices)));
        }

        public Box(Vector3 size) : base(BoxMesh)
        {
            Size = size;
        }
    }
}