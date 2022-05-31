using CourseWork.Common.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.Light
{
    public class SpotLight : PointLight
    {
        private Vector3 _direction;

        public Vector3 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        public float Cutoff { get; set; }

        public override void Apply(int index, ShaderProgram shader)
        {
            Apply(shader, $"spotLight[{index}].");
        }

        protected override void Apply(ShaderProgram shader, string prefix)
        {
            base.Apply(shader, prefix + "base.");
            shader.SetUniform($"{prefix}direction", ref _direction);
            shader.SetUniform($"{prefix}cutoff", (float) MathHelper.Cos(MathHelper.DegreesToRadians(Cutoff)));
        }
    }
}