using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    internal class Image : Figure
    {
        public static readonly Mesh QuadMesh;

        static Image()
        {
            Vertex[] vertices =
            {
                new(new(-1, 1, 0f), new Vector2(0f, 1f)),
                new(new(-1, -1, 0f), new Vector2(0f, 0f)),
                new(new(1, -1, 0f), new Vector2(1f, 0f)),
                new(new(1, 1, 0f), new Vector2(1f, 1f)),
            };

            var indices = new uint[]
            {
                0, 1, 2,
                0, 2, 3
            };

            QuadMesh = new Mesh(GeometryHelper.CalculateNewellNormals(new GeometryData(vertices, indices)));
        }

        public Image(Texture texture) : base(QuadMesh)
        {
            Color = Color4.White;
            Texture = texture;
        }

        public Image() : this(null)
        {
        }

        public static Image FromFile(string filepath)
        {
            return new Image(new Texture(filepath));
        }
    }
}