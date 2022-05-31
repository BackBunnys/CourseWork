using CourseWork.Common.OpenGL;
using CourseWork.Common.Render.Targets;
using CourseWork.Core.AssetManagement;
using CourseWork.Objects;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.Render.Post_Processing
{
    public class BlurEffect
    {
        private readonly RenderTexture[] _renderTextures;
        private readonly Image _image;
        public ShaderProgram Shader { get; set; }

        public Texture Texture => _renderTextures[0].Texture;

        public uint NumberOfIterations { get; set; } = 5;

        public BlurEffect(Vector2i textureSize)
        {
            Shader = AssetManager.GetShader("blurShader");

            _renderTextures = new RenderTexture[2];
            _image = new Image();

            for (var i = 0; i < 2; i++)
            {
                _renderTextures[i] = new RenderTexture(textureSize);
            }
        }

        public Texture Apply(Texture texture)
        {
            for (var i = 0; i < NumberOfIterations; i++)
            {
                Apply(true, _renderTextures[0], i == 0 ? texture : _renderTextures[1].Texture);
                Apply(false, _renderTextures[1], _renderTextures[0].Texture);
            }

            return _renderTextures[0].Texture;
        }

        private void Apply(bool horizontal, RenderTexture renderTexture, Texture texture)
        {
            renderTexture.Clear();
            Shader.Use();
            Shader.SetUniform("horizontal", horizontal ? 1 : 0);
            _image.Draw(renderTexture, RenderStates.Empty with {Texture = texture, Shader = Shader});
            renderTexture.Display();
        }
    }
}