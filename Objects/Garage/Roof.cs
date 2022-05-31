using System.Linq;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Garage
{
    public class Roof : Model
    {
        public Roof(Vector3 size)
        {
            var @base = new Figure(new Mesh(GeometryHelper.CalculateNormals(GeometryHelper.Extrude(new Vector2[]
                {new(1f, -1f), new(0f, 1f), new(-1f, -1f)}, true, 1))))
            {
                Texture = AssetManager.GetTexture("wall"),
                TextureTiling = size,
                Size = new Vector3(0.5f, 0.25f, 0.5f)
            };
            var leftShiver = new Shiver(new Vector3(1, 1f, 0.05f))
            {
                Rotation = new Vector3(45, 90, 90),
                Position = new Vector3(-0.336f, -0.05f, 0)
            };
            var rightShiver = new Shiver(new Vector3(1, 1f, 0.05f))
            {
                Rotation = new Vector3(-45, 90, 90),
                Position = new Vector3(0.336f, -0.05f, 0)
            };

            Drawables.AddRange(new[] {@base, leftShiver, rightShiver});
            InitBounds();
        }
    }
}