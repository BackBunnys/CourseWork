using CourseWork.Common;
using CourseWork.Common.OpenGL;
using OpenTK.Graphics.OpenGL;

namespace CourseWork.Core.AssetManagement
{
    internal class ShaderLoader : IResourceLoader<ShaderProgram, ShaderProgramInfo>
    {
        private readonly ShaderProgramBuilder _builder = new();

        public ShaderProgram Load(ShaderProgramInfo source)
        {
            return _builder.Create()
                .WithShader(ShaderType.VertexShader, source.VertexPath)
                .WithShader(ShaderType.FragmentShader, source.FragmentPath)
                .WithShader(ShaderType.GeometryShader, source.GeometryPath)
                .Build();
        }
    }
}