using CourseWork.Common.OpenGL;
using CourseWork.Common.Render.Targets;
using CourseWork.Core.AssetManagement;
using CourseWork.Objects;
using OpenTK.Mathematics;

namespace CourseWork.Common.Render.Post_Processing
{
    public class BloomEffect
    {
        private readonly RenderTexture _renderTexture;
        private readonly BrightnessEffect _brightnessEffect;
        private readonly BlurEffect _blurEffect;
        private readonly Image _image;
        public ShaderProgram Shader { get; set; }

        public uint NumberOfIterations
        {
            get => _blurEffect.NumberOfIterations;
            set => _blurEffect.NumberOfIterations = value;
        }

        public BloomEffect(Vector2i textureSize, uint numberOfIterations = 5)
        {
            _renderTexture = new RenderTexture(textureSize);
            _brightnessEffect = new BrightnessEffect(textureSize);
            _blurEffect = new BlurEffect(textureSize)
            {
                NumberOfIterations = numberOfIterations
            };
            _image = new Image();
            Shader = AssetManager.GetShader("combineShader");
        }

        public Texture Apply(Texture texture)
        {
            _renderTexture.Clear();

            var brightnessBlur = _blurEffect.Apply(_brightnessEffect.Apply(texture));

            texture.SubTextures.Add(brightnessBlur);
            _image.Draw(_renderTexture, RenderStates.Empty with {Shader = Shader, Texture = texture});
            texture.SubTextures.Remove(brightnessBlur);

            _renderTexture.Display();

            return _renderTexture.Texture;
        }
    }
}