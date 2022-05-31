using System;
using CourseWork.Common;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Targets;
using CourseWork.Core.AssetManagement;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace CourseWork.Objects.Environment
{
    public class Forest : Model
    {
        private readonly uint _count;
        private static readonly Random Random = new(6);
        private Matrix4[] _transforms;

        public Forest(Vector2i size, float density, uint details)
        {
            _count = (uint) (size.X / density + size.Y / density);

            _transforms = new Matrix4[_count];

            var positions = new Vector3[_count];

            for (var i = 0; i < _count; ++i)
            {
                var position = new Vector3(Random.Next(-size.X / 2, size.X / 2), 0,
                    Random.Next(-size.Y / 2, size.Y / 2));
                var rotation = Vector3.Zero;
                var scale = new Vector3(Random.Next(7, 10)) / 10;


                _transforms[i] = GeometryHelper.CreateTransform(position, rotation, scale, Vector3.Zero);
                positions[i] = position;
            }

            Shader = AssetManager.GetShader("instancedShader");

            var tree = new Tree(details)
            {
                Position = new Vector3(0, 1.5f, 0)
            };
            Drawables.Add(tree);

            LocalBounds = GeometryHelper.CalculateBounds(positions).Inflated(new Vector3(0, tree.Bounds.Size.Y, 0));
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.LightContext.Apply(Shader);

            base.Draw(target, states with {Transforms = _transforms});
        }
    }
}