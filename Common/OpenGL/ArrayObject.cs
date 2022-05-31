using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CourseWork.Common.Render.Drawables;
using OpenTK.Graphics.OpenGL;

namespace CourseWork.Common.OpenGL
{
    public class ArrayObject : Bindable, IDrawable, IDisposable
    {
        public readonly int Handle;

        public readonly IList<BufferObject> Buffers = new List<BufferObject>();

        private bool _elementsArrayUses;
        private uint _pointerCount;
        public int Length { get; private set; }
        public PrimitiveType Mode { get; set; } = PrimitiveType.Triangles;

        public ArrayObject()
        {
            Handle = GL.GenVertexArray();
        }

        ~ArrayObject() => Cleanup();

        public static ArrayObject ForBuffers(IEnumerable<BufferObject> buffers)
        {
            ArrayObject arrayObject = new();

            foreach (var buffer in buffers)
                arrayObject.AttachBuffer(buffer);

            return arrayObject;
        }

        public override void Bind() => GL.BindVertexArray(Handle);

        public override void Unbind() => GL.BindVertexArray(0);

        public ArrayObject AttachBuffer(BufferObject buffer)
        {
            RunWithBinding(buffer.Bind);

            Buffers.Add(buffer);

            if (_elementsArrayUses) return this;

            if (buffer.Type is BufferTarget.ArrayBuffer or BufferTarget.ElementArrayBuffer && !_elementsArrayUses)
                Length = (int) buffer.Length;

            if (buffer.Type == BufferTarget.ElementArrayBuffer)
                _elementsArrayUses = true;

            return this;
        }

        public void Draw(uint numberOfInstances = 1)
        {
            RunWithBinding(() =>
            {
                Statistics.DrawCalls += 1;
                Statistics.Indices += (_elementsArrayUses ? (uint) Length : 0u) * numberOfInstances;
                Statistics.Vertices += Buffers[0].Length * numberOfInstances;

                if (numberOfInstances > 1)
                {
                    if (_elementsArrayUses)
                        GL.DrawElementsInstanced(Mode, Length, DrawElementsType.UnsignedInt, (IntPtr) 0,
                            (int) numberOfInstances);
                    else GL.DrawArraysInstanced(Mode, 0, Length, (int) numberOfInstances);
                }
                else
                {
                    if (_elementsArrayUses) GL.DrawElements(Mode, Length, DrawElementsType.UnsignedInt, 0);
                    else GL.DrawArrays(Mode, 0, Length);
                }
            });
        }

        public void ConfigureDivisor(uint position, uint divisor)
        {
            RunWithBinding(() => GL.VertexAttribDivisor(position, divisor));
        }

        public ArrayObject AttachPointer<T>(string name, uint divisor = 0) where T : struct
        {
            ConfigureDivisor(_pointerCount, divisor);
            return AttachPointer<T>(name, (int) _pointerCount);
        }

        public ArrayObject AttachPointer<T>(string name, int position) where T : struct
        {
            var fieldType = typeof(T).GetField(name)?.FieldType;
            if (fieldType == null) throw new ArgumentException($"Cannot access field {name} of {nameof(T)} type");

            var size = Marshal.SizeOf(fieldType) / 4;
            return AttachPointer<T>(name, size, VertexAttribPointerType.Float, position);
        }

        public ArrayObject AttachPointer<T>(string name, int size, VertexAttribPointerType type, int position)
            where T : struct
        {
            return AttachPointer(position, size, type, Marshal.SizeOf<T>(), Marshal.OffsetOf(typeof(T), name));
        }

        public ArrayObject AttachPointer(int position, int size, VertexAttribPointerType type, int stride,
            IntPtr pointer)
        {
            RunWithBinding(() =>
            {
                GL.VertexAttribPointer(position, size, type, false, stride,
                    pointer);
                GL.EnableVertexAttribArray(position);
            });

            ++_pointerCount;

            return this;
        }

        public void Dispose()
        {
            Cleanup();

            GC.SuppressFinalize(this);
        }

        private void Cleanup()
        {
            Unbind();

            GL.DeleteVertexArray(Handle);

            foreach (var buffer in Buffers)
                buffer.Dispose();
        }
    }
}