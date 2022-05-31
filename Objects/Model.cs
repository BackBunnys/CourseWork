using System;
using System.Collections.Generic;
using System.Linq;
using CourseWork.Common;
using CourseWork.Common.Geometry;
using CourseWork.Common.OpenGL;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Targets;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class Model : Transformable, IDrawableGeometry
    {
        public List<IDrawableGeometry> Drawables = new();

        public Box3 LocalBounds { get; protected set; }
        public Box3 Bounds { get; protected set; }

        public ShaderProgram Shader { get; set; }

        public virtual IIntersectable Intersects(IIntersectable intersectable)
        {
            if (!Bounds.Contains(intersectable.Bounds)) return null;

            var intersectableBounds = intersectable.Bounds;
            intersectableBounds.Translate(-Position);
            intersectableBounds.Scale(Vector3.One / Size, Vector3.Zero);

            return Drawables.FirstOrDefault(drawable => drawable.Bounds.Contains(intersectableBounds)) != null
                ? this
                : null;
        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            var localStates = states with
            {
                Transform = TransformMatrix * states.Transform, Shader = Shader ?? states.Shader
            };
            foreach (var drawable in Drawables)
            {
                drawable.Draw(target, localStates);
            }
        }

        protected void InitBounds()
        {
            RecalculateLocalBounds();
            Drawables.ForEach(d => d.Changed += RecalculateLocalBounds);
        }

        protected override void OnChanged()
        {
            base.OnChanged();
            RecalculateBounds();
        }

        protected void RecalculateLocalBounds()
        {
            var boundPoints = new List<Vector3>();

            foreach (var drawable in Drawables)
            {
                boundPoints.AddRange(drawable.Bounds.Points());
            }

            LocalBounds = GeometryHelper.CalculateBounds(boundPoints);

            OnChanged();
        }

        protected void RecalculateBounds()
        {
            var boundPoints = LocalBounds.Points().Select(p => (TransformMatrix * new Vector4(p)).Xyz);

            Bounds = GeometryHelper.CalculateBounds(boundPoints.ToList())
                .Translated(TransformMatrix.ExtractTranslation());
        }
    }
}