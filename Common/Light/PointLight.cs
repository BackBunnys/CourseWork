using CourseWork.Common.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.Light
{
    public struct Attenuation
    {
        public float Constant;
        public float Linear;
        public float Exp;
    }

    public class PointLight : BaseLight
    {
        private Vector3 _position;

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public Attenuation Attenuation { get; set; }

        public override void Apply(int index, ShaderProgram shader)
        {
            Apply(shader, $"pointLight[{index}].");
        }

        protected override void Apply(ShaderProgram shader, string prefix)
        {
            base.Apply(shader, prefix + "base.");
            shader.SetUniform($"{prefix}position", ref _position);
            shader.SetUniform($"{prefix}attenuation.constant", Attenuation.Constant);
            shader.SetUniform($"{prefix}attenuation.linear", Attenuation.Linear);
            shader.SetUniform($"{prefix}attenuation.exp", Attenuation.Exp);
        }
    }
}