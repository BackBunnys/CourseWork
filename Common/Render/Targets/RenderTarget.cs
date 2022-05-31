using CourseWork.Common.Light;
using CourseWork.Common.Render.Drawables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.Render.Targets
{
    public enum RenderMode
    {
        Lines = PolygonMode.Line,
        Fill = PolygonMode.Fill
    }

    public abstract class RenderTarget
    {
        public Camera.Camera Camera;
        public Matrix4 Projection;
        public Vector2i Size;
        public LightContext LightContext;
        public RenderMode RenderMode = RenderMode.Fill;

        protected RenderTarget(Vector2i size) : this(new Camera.Camera(), Matrix4.Identity, size)
        {
        }

        public RenderTarget(Camera.Camera camera, Matrix4 projection, Vector2i size)
        {
            Camera = camera;
            Projection = projection;
            Size = size;
        }

        public virtual void Draw(IDrawable drawable, RenderStates states)
        {
            PrepareDrawCall(states);
            drawable.Draw(states.NumberOfInstances);
            FinalizeDrawCall(states);
        }

        public virtual void Clear(Color4 color)
        {
            GL.ClearColor(color);
            Clear();
        }

        public virtual void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public abstract void Display();

        private void PrepareDrawCall(RenderStates states)
        {
            if (states.Shader != null)
            {
                states.Shader.Use();

                var transform = states.Transform;
                var view = Camera.TransformMatrix;
                var viewPosition = -Camera.Position;
                var color = states.Color;

                states.Shader.SetUniform("model", ref transform);
                states.Shader.SetUniform("view", ref view);
                states.Shader.SetUniform("viewPosition", ref viewPosition);
                states.Shader.SetUniform("projection", ref Projection);
                states.Shader.SetUniform("color", ref color);

                var textureTiling = states.TextureTiling;
                states.Shader.SetUniform("textureTiling", ref textureTiling);

                states.Shader.SetUniform("colorMixing", states.ColorMixing);
            }

            if (states.Texture != null)
            {
                states.Texture.Use(TextureUnit.Texture0);
                var subTextures = states.Texture.SubTextures;

                for (var i = 0; i < subTextures.Count; ++i)
                {
                    states.Shader.SetUniform($"textureUnit{i + 1}", 1);
                    subTextures[i].Use(TextureUnit.Texture1 + i);
                }
            }

            GL.PolygonMode(MaterialFace.FrontAndBack, (PolygonMode)RenderMode);
        }

        private void FinalizeDrawCall(RenderStates states)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            if (states.Texture != null)
            {
                states.Texture.Disable();
                var subTextures = states.Texture.SubTextures;

                for (var i = 0; i < subTextures.Count; ++i)
                {
                    states.Shader.SetUniform($"textureUnit{i + 1}", 0);
                    subTextures[i].Disable();
                }
            }

            states.Shader?.Disable();
        }
    }
}