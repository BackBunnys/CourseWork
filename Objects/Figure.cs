using System.Linq;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using CourseWork.Common.Render.Targets;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class Figure : TargetDrawable<Mesh>, IDrawableGeometry
    {
        public Box3 LocalBounds { get; protected set; }

        public Box3 Bounds { get; protected set; }

        public virtual IIntersectable Intersects(IIntersectable intersectable)
        {
            return Bounds.Contains(intersectable.Bounds) ? this : null;
        }

        public Figure(Mesh mesh) : base(mesh)
        {
            LocalBounds = GeometryHelper.CalculateBounds(mesh.Vertices);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (states.Transforms is { Length: > 0 })
            {
                Drawable.Tbo.SetData(states.Transforms);
                base.Draw(target, states with { NumberOfInstances = (uint)states.Transforms.Length });
            }
            else
            {
                base.Draw(target, states);
            }
        }

        protected override void OnChanged()
        {
            base.OnChanged();
            RecalculateBounds();
        }

        private void RecalculateBounds()
        {
            var boundPoints = LocalBounds.Points().Select(p => (new Vector4(p) * TransformMatrix).Xyz);

            Bounds = GeometryHelper.CalculateBounds(boundPoints.ToList())
                .Translated(TransformMatrix.ExtractTranslation());
        }
    }
}