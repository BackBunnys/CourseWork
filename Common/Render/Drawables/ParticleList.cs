using System;
using CourseWork.Common.Geometry;
using CourseWork.Common.OpenGL;
using OpenTK.Graphics.OpenGL;

namespace CourseWork.Common.Render.Drawables
{
    public class ParticleList : IDrawable, IDisposable
    {
        public readonly ArrayObject Vao;
        public readonly BufferObject Vbo;

        public ParticleList(uint size)
        {
            Vao = new ArrayObject();
            Vbo = new BufferObject(BufferTarget.ArrayBuffer);
            Vbo.Allocate<Particle>(size);

            Vao
                .AttachBuffer(Vbo)
                    .AttachPointer<Particle>(nameof(Particle.Position))
                    .AttachPointer<Particle>(nameof(Particle.Lifetime))
                    .AttachPointer<Particle>(nameof(Particle.Size))
                    .AttachPointer<Particle>(nameof(Particle.Type));

            Vao.Mode = PrimitiveType.Points;
        }

        ~ParticleList() => Cleanup();

        public void Draw(uint numberOfInstances)
        {
            Vao.Draw(numberOfInstances);
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