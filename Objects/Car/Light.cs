using CourseWork.Common;
using CourseWork.Common.Light;
using CourseWork.Common.OpenGL;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Targets;
using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Car
{
    public class Light : Box, ILightSource
    {
        private readonly SpotLight _light = new()
        {
            Color = new Vector3(1),
            AmbientIntensity = 0,
            DiffuseIntensity = 1f,
            Attenuation = new Attenuation()
            {
                Constant = 1f,
                Linear = 0.01f,
                Exp = 0.01f
            },
            Cutoff = 60f,
            Direction = Vector3.UnitX
        };

        private readonly ShaderProgram _lightShader;

        public float Intensity
        {
            get => _light.DiffuseIntensity;
            set => _light.DiffuseIntensity = value;
        }

        public Light(Vector3 size) : base(size)
        {
            Color = Color4.White;
            _lightShader = AssetManager.GetShader("lightShader");
            LightContext = LightContext.Of(_light);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            Shader = _light.Enabled ? _lightShader : states.Shader;
            Shader.Use();
            Shader.SetUniform("intensity", Intensity * 4);
            var localStates = states with
            {
                Transform = TransformMatrix * states.Transform,
            };

            base.Draw(target, states);

            _light.Color = new Vector3(Color.R, Color.G, Color.B);
            _light.Position = localStates.Transform.ExtractTranslation();
            _light.Direction = localStates.Transform.ExtractRotation() * Vector3.UnitX;
        }

        public LightContext LightContext { get; }
    }
}