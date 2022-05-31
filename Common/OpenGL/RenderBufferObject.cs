using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.OpenGL
{
    public class RenderBufferObject : Bindable
    {
        public readonly int Handle;

        public RenderBufferObject()
        {
            Handle = GL.GenRenderbuffer();
        }

        public RenderBufferObject(Vector2i size, RenderbufferStorage storage = RenderbufferStorage.DepthComponent) :
            this()
        {
            RunWithBinding(() => Storage(size, storage));
        }

        public RenderBufferObject(Vector2i size, int samples,
            RenderbufferStorage storage = RenderbufferStorage.DepthComponent) : this()
        {
            RunWithBinding(() => StorageMultiSampled(size, samples, storage));
        }

        public override void Bind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);
        }

        ~RenderBufferObject()
        {
            GL.DeleteRenderbuffer(Handle);
        }

        public override void Unbind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }

        public void Storage(Vector2i size, RenderbufferStorage storage = RenderbufferStorage.DepthComponent)
        {
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,
                RenderbufferStorage.DepthComponent,
                size.X,
                size.Y);
        }

        public void StorageMultiSampled(Vector2i size, int samples,
            RenderbufferStorage storage = RenderbufferStorage.DepthComponent)
        {
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, samples,
                storage,
                size.X,
                size.Y);
        }
    }
}