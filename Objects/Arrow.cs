using System.Linq;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class Arrow : Figure
    {
        public static readonly Mesh ArrowMesh;

        static Arrow()
        {
            var canvasPoints = new Vector2[]
            {
                new(0, -0.5f),
                new(-0.5f, 0f),
                new(-0.2f, 0f),
                new(-0.2f, 0.5f),
                new(0.2f, 0.5f),
                new(0.2f, 0f),
                new(0.5f, 0f),
                new(0, -0.5f)
            };

            ArrowMesh = new Mesh(GeometryHelper.CalculateNewellNormals(
                GeometryHelper.Extrude(canvasPoints.Reverse().ToList())
            ));
        }

        public Arrow() : base(ArrowMesh)
        {
            Size = new Vector3(2, 2, 0.1f);
        }
    }
}