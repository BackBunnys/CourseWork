using CourseWork.Common.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.Light
{
    public class BaseLight : ILight
    {
        protected Vector3 _color;

        public Vector3 Color
        {
            get => _color;
            set => _color = value;
        }

        public float AmbientIntensity { get; set; }
        public float DiffuseIntensity { get; set; }
        public bool Enabled { get; set; } = true;

        public virtual void Apply(int index, ShaderProgram shader)
        {
            Apply(shader, $"base[{index}].");
        }

        public void Apply(ShaderProgram shader)
        {
            Apply(shader, "");
        }

        protected virtual void Apply(ShaderProgram shader, string prefix)
        {
            shader.Use();
            shader.SetUniform($"{prefix}color", ref _color);
            shader.SetUniform($"{prefix}ambientIntensity", AmbientIntensity);
            shader.SetUniform($"{prefix}diffuseIntensity", DiffuseIntensity);
        }
    }
}