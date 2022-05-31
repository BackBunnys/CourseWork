using System.Collections.Generic;
using System.Linq;
using CourseWork.Common.Render;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.OpenGL
{
    public class FramebufferObject : Bindable
    {
        private ReadBufferMode _readMode = ReadBufferMode.ColorAttachment0;
        private DrawBufferMode _drawMode = DrawBufferMode.ColorAttachment0;

        public readonly int Handle;

        private IDictionary<FramebufferAttachment, Texture> _attachments =
            new Dictionary<FramebufferAttachment, Texture>();

        public ReadBufferMode ReadMode
        {
            get => _readMode;
            set
            {
                _readMode = value; 
                RunWithBinding(() => GL.ReadBuffer(_readMode));
            }
        }

        public DrawBufferMode DrawMode
        {
            get => _drawMode;
            set
            {
                _drawMode = value;
                RunWithBinding(() => GL.DrawBuffer(_drawMode));
            }
        }

        private RenderBufferObject _depthRenderBuffer;

        public FramebufferObject()
        {
            Handle = GL.GenFramebuffer();
        }

        ~FramebufferObject()
        {
            Unbind();
            GL.DeleteFramebuffer(Handle);
        }

        public static FramebufferObject Of(Texture texture,
            FramebufferAttachment attachment = FramebufferAttachment.ColorAttachment0)
        {
            return new FramebufferObject().Attach(texture, attachment);
        }

        public static FramebufferObject Of(Texture texture, RenderBufferObject renderBuffer)
        {
            return Of(texture).Attach(renderBuffer);
        }

        public FramebufferObject Attach(Texture texture,
            FramebufferAttachment attachment = FramebufferAttachment.ColorAttachment0)
        {
            RunWithBinding(() =>
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, texture.Type, texture.Handle, 0);
                if (attachment is >= FramebufferAttachment.ColorAttachment0
                    and <= FramebufferAttachment.ColorAttachment15) //fixme: can be 31, but render up to 15 
                    ProcessColorAttachment(attachment, texture);
            });
            return this;
        }

        public FramebufferObject Attach(RenderBufferObject renderBuffer,
            FramebufferAttachment attachment = FramebufferAttachment.DepthAttachment)
        {
            RunWithBinding(() =>
            {
                renderBuffer.Bind();
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, attachment,
                    RenderbufferTarget.Renderbuffer, renderBuffer.Handle);
                renderBuffer.Unbind();
            });

            _depthRenderBuffer = renderBuffer;
            return this;
        }

        public void CopyTo(FramebufferObject anotherFramebuffer, Box2i srcRect, Box2i dstRect,
            ClearBufferMask clearMask = ClearBufferMask.ColorBufferBit,
            BlitFramebufferFilter filter = BlitFramebufferFilter.Nearest)
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, Handle);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, anotherFramebuffer.Handle);

            GL.BlitFramebuffer(
                srcRect.Min.X, srcRect.Min.Y, srcRect.Max.X, srcRect.Max.Y,
                dstRect.Min.X, dstRect.Min.Y, dstRect.Max.X, dstRect.Max.Y,
                clearMask, filter
            );

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public override void Bind() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);

        public override void Unbind() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        private void ProcessColorAttachment(FramebufferAttachment attachment, Texture texture)
        {
            _attachments.Add(attachment, texture);
            GL.DrawBuffers(_attachments.Count,
                _attachments.Select((a) => (DrawBuffersEnum) a.Key).ToArray());
        }
    }
}