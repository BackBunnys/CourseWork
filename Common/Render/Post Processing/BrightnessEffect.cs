using CourseWork.Common.OpenGL;
using CourseWork.Common.Render.Targets;
using CourseWork.Core.AssetManagement;
using CourseWork.Objects;
using OpenTK.Mathematics;

namespace CourseWork.Common.Render.Post_Processing
{
    public class BrightnessEffect
    {
        private readonly RenderTexture _renderTexture;
        private ShaderProgram Shader { get; set; }

        private readonly Image _image;

        public BrightnessEffect(Vector2i size)
        {
            Shader = AssetManager.GetShader("brightnessShader");
            _renderTexture = new RenderTexture(size);
            _renderTexture.EnableDepth();
            _image = new Image();
        }

        public Texture Apply(Texture texture)
        {
            _renderTexture.Clear();
            _image.Draw(_renderTexture, RenderStates.Empty with {Texture = texture, Shader = Shader});
            _renderTexture.Display();
            return _renderTexture.Texture;
        }
    }
}