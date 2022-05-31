using CourseWork.Common.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.Light
{
    public class DirectionalLight : BaseLight
    {
        private Vector3 _direction;
        public Vector3 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        public override void Apply(int index, ShaderProgram shader)
        {
            Apply(shader, $"directionalLight[{index}].");
        }

        protected override void Apply(ShaderProgram shader, string prefix)
        {
            base.Apply(shader, prefix + "base.");
            shader.SetUniform($"{prefix}direction", ref _direction);
        }
    }
}