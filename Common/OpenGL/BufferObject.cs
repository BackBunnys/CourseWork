using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace CourseWork.Common.OpenGL
{
    public class BufferObject : Bindable, IDisposable
    {
        public readonly int Handle;

        public BufferTarget Type { get; set; }
        public uint Length { get; private set; }

        public static BufferObject ОfData<T>(IEnumerable<T> data, BufferTarget bufferTarget,
            BufferUsageHint hint = BufferUsageHint.StaticDraw) where T : struct
        {
            BufferObject buffer = new(bufferTarget);
            buffer.SetData(data.ToArray(), hint);

            return buffer;
        }

        public BufferObject(BufferTarget bufferTarget)
        {
            Type = bufferTarget;
            Handle = GL.GenBuffer();
        }

        ~BufferObject() => Cleanup();

        public void SetData<T>(T[] data, BufferUsageHint hint = BufferUsageHint.StaticDraw) where T : struct
        {
            RunWithBinding(() => GL.BufferData(Type, Marshal.SizeOf(typeof(T)) * data.Length, data, hint));

            Length = (uint) data.Length;
        }

        public void Allocate<T>(uint size, BufferUsageHint hint = BufferUsageHint.DynamicDraw) where T : struct
        {
            RunWithBinding(() => GL.BufferData(Type, (int) (Marshal.SizeOf(typeof(T)) * size), (IntPtr) null, hint));

            Length = size;
        }

        public void BindBase(BufferRangeTarget target, int position = 0)
        {
            GL.BindBufferBase(target, position, Handle);
        }

        public override void Bind()
        {
            GL.BindBuffer(Type, Handle);
        }

        public override void Unbind()
        {
            GL.BindBuffer(Type, 0);
        }

        public void Dispose()
        {
            Cleanup();

            GC.SuppressFinalize(this);
        }

        private void Cleanup()
        {
            Unbind();

            GL.DeleteBuffer(Handle);
        }
    }
}