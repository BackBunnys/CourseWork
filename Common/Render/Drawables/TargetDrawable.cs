using CourseWork.Common.OpenGL;
using CourseWork.Common.Render.Targets;
using OpenTK.Mathematics;

namespace CourseWork.Common.Render.Drawables
{
    public abstract class TargetDrawable<T> : Transformable, ITargetDrawable where T : IDrawable
    {
        public readonly T Drawable;

        protected TargetDrawable(T drawable)
        {
            this.Drawable = drawable;
        }

        public Texture Texture { get; set; }
        public Vector3 TextureTiling { get; set; } = Vector3.One;
        public ShaderProgram Shader { get; set; }

        public Color4 Color { get; set; }
        public float ColorMixing { get; set; }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Drawable, states with
            {
                Transform = TransformMatrix * states.Transform,
                Texture = Texture ?? states.Texture,
                TextureTiling = TextureTiling,
                Shader = Shader ?? states.Shader,
                Color = Color,
                ColorMixing = Texture != null ? ColorMixing : 1f,
                NumberOfInstances = states.NumberOfInstances
            });
        }
    }
}