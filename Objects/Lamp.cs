using CourseWork.Common;
using CourseWork.Common.Light;
using CourseWork.Common.OpenGL;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Targets;
using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class Lamp : Box, ILightSource
    {
        private readonly PointLight _light = new()
        {
            Color = new Vector3(1),
            AmbientIntensity = 0,
            DiffuseIntensity = 1f,
            Attenuation = new Attenuation()
            {
                Constant = 1f,
                Linear = 0.5f,
                Exp = 0.2f
            }
        };

        private readonly ShaderProgram _lightShader;

        public LightContext LightContext { get; }

        public Lamp(Vector3 size) : base(size)
        {
            _lightShader = AssetManager.GetShader("lightShader");
            Color = Color4.White;
            LightContext = LightContext.Of(_light);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            Shader = _light.Enabled ? _lightShader : states.Shader;
            Shader.Use();
            Shader.SetUniform("intensity", LightContext.Enabled ? 2f : 0f);
            base.Draw(target, states);

            _light.Position = (TransformMatrix * states.Transform).ExtractTranslation();
        }
    }
}