using CourseWork.Common.OpenGL;

namespace CourseWork.Common.Light
{
    public interface ILight
    {
        public bool Enabled { get; set; }
        void Apply(int index, ShaderProgram shader);

        void Apply(ShaderProgram shader);
    }
}