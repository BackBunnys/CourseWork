using System;
using System.Collections.Generic;
using CourseWork.Common.Math;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.OpenGL
{
    public class ShaderProgram : IDisposable
    {
        public readonly int Handle;

        private bool _disposedValue;

        private readonly IDictionary<string, int> _uniformLocations = new Dictionary<string, int>();

        public ShaderProgram(int handle)
        {
            Handle = handle;
            //InitUniforms();
        }

        ~ShaderProgram()
        {
            Dispose(false);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void Disable()
        {
            GL.UseProgram(0);
        }

        public int GetAttributeLocation(string attributeName)
        {
            return GL.GetAttribLocation(Handle, attributeName);
        }

        public int GetUniformLocation(string uniformName)
        {
           // if (_uniformLocations.TryGetValue(uniformName, out var location))
           //     return location;
            return GL.GetUniformLocation(Handle, uniformName);
        }

        public void SetUniform(string name, ref Matrix4 matrix, bool force = false)
        {
            var location = GetUniformLocation(name);
            if (location < 0 && force)
            {
                location = GL.GetUniformLocation(Handle, name);
            }

            SetUniform(location, ref matrix);
        }

        public void SetUniform(string name, ref Color4 color)
        {
            var location = GetUniformLocation(name);
            SetUniform(location, ref color);
        }

        public void SetUniform(int location, ref Color4 color)
        {
            if (location < 0) return;
            GL.Uniform4(location, color);
        }

        public void SetUniform(string name, float value)
        {
            var location = GetUniformLocation(name);
            SetUniform(location, value);
        }

        public void SetUniform(int location, float value)
        {
            if (location < 0) return;
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name, int value)
        {
            var location = GetUniformLocation(name);
            SetUniform(location, value);
        }

        public void SetUniform(int location, int value)
        {
            if (location < 0) return;
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name, Vector2d point)
        {
            var location = GetUniformLocation(name);
            SetUniform(location, point);
        }

        public void SetUniform(int location, Vector2d point)
        {
            if (location < 0) return;
            var (x, y) = point;
            GL.Uniform2(location, x, y);
        }

        public void SetUniform(string name, ref Vector3 point)
        {
            var location = GetUniformLocation(name);
            SetUniform(location, ref point);
        }

        public void SetUniform(int location, ref Vector3 point)
        {
            if (location < 0) return;
            var (x, y, z) = point;
            GL.Uniform3(location, x, y, z);
        }

        public void SetUniform(string name, IEnumerable<Vector3> points)
        {
            SetUniform(name, points.Raw().ToArray());
        }

        public void SetUniform(string name, float[] rawPoints)
        {
            var location = GetUniformLocation(name);
            SetUniform(location, rawPoints);
        }

        public void SetUniform(int location, float[] rawPoints)
        {
            if (location < 0) return;
            GL.Uniform3(location, rawPoints.Length / 3, rawPoints);
        }

        public void SetUniform(int location, ref Matrix4 matrix)
        {
            if (location < 0) return;
            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void SetUniform(string name, IEnumerable<Matrix4> matrices)
        {
            SetUniformM(name, matrices.Raw().ToArray());
        }

        public void SetUniformM(string name, float[] rawMatrices)
        {
            var location = GetUniformLocation(name);
            SetUniform(location, rawMatrices);
        }

        public void SetUniformM(int location, float[] rawMatrices)
        {
            if (location < 0) return;
            GL.UniformMatrix4(location, rawMatrices.Length / 16, false, rawMatrices);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            GL.DeleteProgram(Handle);

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void InitUniforms()
        {
            GL.GetProgramInterface(Handle, ProgramInterface.Uniform, ProgramInterfaceParameter.ActiveResources,
                out var count);

            ProgramProperty[] properties = {ProgramProperty.NameLength};
            var param = new int[properties.Length];
            for (var i = 0; i < count; ++i)
            {
                GL.GetProgramResource(Handle, ProgramInterface.Uniform, i, properties.Length, properties, 1, out _,
                    param);
                GL.GetProgramResourceName(Handle, ProgramInterface.Uniform, i, param[0], out _, out var name);
                _uniformLocations.Add(name, i);
            }
        }
    }
}