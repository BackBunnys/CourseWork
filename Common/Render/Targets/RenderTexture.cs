using CourseWork.Common.OpenGL;
using CourseWork.Common.Render.Drawables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.Render.Targets
{
    public class RenderTexture : RenderTarget
    {
        private FramebufferObject _msFramebuffer;
        private readonly FramebufferObject _framebuffer;
        private RenderBufferObject _renderBuffer;

        private int _samples;

        public Texture Texture { get; }

        public RenderTexture(Camera.Camera camera, Matrix4 projection, Vector2i size) : base(camera,
            projection, size)
        {
            Texture = Texture.Create(size);
            _framebuffer = FramebufferObject.Of(Texture);
        }

        public RenderTexture(RenderTarget target) : this(target.Camera, target.Projection,
            target.Size)
        {
        }

        public RenderTexture(Vector2i size) : this(new Camera.Camera(Vector3.Zero), Matrix4.Identity, size)
        {
        }

        public static RenderTexture CreateDepth(Vector2i size)
        {
            return new RenderTexture(
                Texture.Create(size, PixelInternalFormat.DepthComponent, PixelFormat.DepthComponent),
                FramebufferAttachment.DepthAttachment);
        }

        public RenderTexture(Texture texture, FramebufferAttachment attachment) : base(texture.Size)
        {
            Texture = texture;
            _framebuffer = FramebufferObject.Of(texture, attachment);
            _framebuffer.DrawMode = DrawBufferMode.None;
            _framebuffer.ReadMode = ReadBufferMode.None;
        }

        public void EnableMultiSampling(int samples)
        {
            _samples = samples;
            _msFramebuffer = new FramebufferObject();
            var texture = Texture.CreateMultiSampled(Size, _samples);
            _msFramebuffer.Attach(texture);
        }

        public void EnableDepth()
        {
            _renderBuffer = _msFramebuffer == null
                ? new RenderBufferObject(Size)
                : new RenderBufferObject(Size, _samples);

            if (_msFramebuffer == null)
                _framebuffer.Attach(_renderBuffer);
            else
                _msFramebuffer.Attach(_renderBuffer);
        }

        public override void Draw(IDrawable drawable, RenderStates states)
        {
            Activate();
            base.Draw(drawable, states);
            Deactivate();
        }

        public void Activate()
        {
            var buffer = _msFramebuffer ?? _framebuffer;
            buffer.Bind();
            if (_renderBuffer == null)
            {
                GL.Disable(EnableCap.DepthTest);
            }
        }

        public void Deactivate()
        {
            var buffer = _msFramebuffer ?? _framebuffer;
            if (_renderBuffer == null)
            {
                GL.Enable(EnableCap.DepthTest);
            }

            buffer.Unbind();
        }

        public override void Clear()
        {
            var buffer = _msFramebuffer ?? _framebuffer;
            buffer.Bind();
            base.Clear();
            buffer.Unbind();
        }

        public override void Display()
        {
            _msFramebuffer?.CopyTo(_framebuffer, new Box2i(Vector2i.Zero, Size),
                new Box2i(Vector2i.Zero, Size));
        }
    }
}