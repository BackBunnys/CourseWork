using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace CourseWork.Common.OpenGL
{
    public class ShaderProgramBuilder
    {
        private string[] _varyings;
        private TransformFeedbackMode _mode;
        private IDictionary<ShaderType, int> _shaderDictionary;

        public ShaderProgramBuilder Create()
        {
            _varyings = null;
            _mode = TransformFeedbackMode.InterleavedAttribs;
            _shaderDictionary = new Dictionary<ShaderType, int>();

            return this;
        }

        public ShaderProgramBuilder WithShader(ShaderType type, string path)
        {
            if (path == null) return this;

            _shaderDictionary[type] = CreateShader(path, type);
            return this;
        }

        public ShaderProgramBuilder WithTransformVaryings(string[] varyings,
            TransformFeedbackMode mode = TransformFeedbackMode.InterleavedAttribs)
        {
            _varyings = varyings;
            _mode = mode;
            return this;
        }

        public ShaderProgram Build()
        {
            var handle = GL.CreateProgram();

            foreach (var shaderHandle in _shaderDictionary.Values)
            {
                GL.AttachShader(handle, shaderHandle);
            }

            if (_varyings != null)
                GL.TransformFeedbackVaryings(handle, _varyings.Length, _varyings, _mode);

            GL.LinkProgram(handle);

            foreach (var shaderHandle in _shaderDictionary.Values)
            {
                GL.DetachShader(handle, shaderHandle);
                GL.DeleteShader(shaderHandle);
            }

            return new ShaderProgram(handle);
        }

        public static int CreateShader(string filepath, ShaderType type)
        {
            string shaderSource;

            using (StreamReader reader = new(filepath, Encoding.UTF8))
            {
                shaderSource = reader.ReadToEnd();
            }

            var shader = GL.CreateShader(type);
            GL.ShaderSource(shader, shaderSource);

            GL.CompileShader(shader);

            var infoLogVertex = GL.GetShaderInfoLog(shader);
            if (infoLogVertex != string.Empty)
                Console.WriteLine(infoLogVertex);

            return shader;
        }
    }
}