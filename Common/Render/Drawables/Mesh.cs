using System;
using System.Collections.Generic;
using System.Linq;
using CourseWork.Common.Geometry;
using CourseWork.Common.OpenGL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.Render.Drawables
{
    public class Mesh : IDrawable, IDisposable
    {
        public readonly ArrayObject Vao;
        public readonly BufferObject Vbo;
        public readonly BufferObject Ebo;
        public readonly BufferObject Tbo;

        public readonly IEnumerable<Vertex> Vertices;

        public Mesh(IEnumerable<Vertex> vertices, IEnumerable<uint> indices,
            BufferUsageHint usage = BufferUsageHint.StaticDraw)
        {
            var data = vertices as Vertex[] ?? vertices.ToArray();
            Vertices = data;
            Vao = new ArrayObject();
            Vbo = BufferObject.ОfData(data, BufferTarget.ArrayBuffer, usage);
            Tbo = new BufferObject(BufferTarget.ArrayBuffer);
            Ebo = BufferObject.ОfData(indices, BufferTarget.ElementArrayBuffer, usage);

            Vao.AttachBuffer(Vbo)
                    .AttachPointer<Vertex>(nameof(Vertex.Position))
                    .AttachPointer<Vertex>(nameof(Vertex.TextureCoords))
                    .AttachPointer<Vertex>(nameof(Vertex.Normal))
                    .AttachPointer<Vertex>(nameof(Vertex.Tangent))
                .AttachBuffer(Tbo)
                    .AttachPointer<Matrix4>(nameof(Matrix4.Row0), 1u)
                    .AttachPointer<Matrix4>(nameof(Matrix4.Row1), 1u)
                    .AttachPointer<Matrix4>(nameof(Matrix4.Row2), 1u)
                    .AttachPointer<Matrix4>(nameof(Matrix4.Row3), 1u)
                .AttachBuffer(Ebo);
        }

        public Mesh(GeometryData geometryData, BufferUsageHint usage = BufferUsageHint.StaticDraw) : this(
            geometryData.Vertices, geometryData.Indices, usage)
        {
        }

        ~Mesh() => Cleanup();

        public void Draw(uint numberOfInstances)
        {
            Vao.Draw(numberOfInstances);
        }

        public void Draw(Matrix4[] transforms)
        {
            Tbo.SetData(transforms, BufferUsageHint.DynamicDraw);
            Vao.Draw((uint) transforms.Length);
        }

        public void Dispose()
        {
            Cleanup();

            GC.SuppressFinalize(this);
        }

        private void Cleanup()
        {
            Vao?.Dispose();
        }
    }
}