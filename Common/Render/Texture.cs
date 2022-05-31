using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using CourseWork.Common.OpenGL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace CourseWork.Common.Render
{
    public class Texture : Bindable, IDisposable
    {
        public readonly int Handle;
        public readonly TextureTarget Type;
        public Vector2i Size { get; private set; }

        public IList<Texture> SubTextures { get; } = new List<Texture>();

        public Texture(string path, TextureTarget type = TextureTarget.Texture2D) : this(type)
        {
            RunWithBinding(() =>
            {
                using (var image = new Bitmap(path))
                {
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    Size = new Vector2i(image.Size.Width, image.Size.Height);

                    var data = image.LockBits(
                        new Rectangle(0, 0, image.Width, image.Height),
                        ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D(Type,
                        0,
                        PixelInternalFormat.Rgba,
                        image.Width,
                        image.Height,
                        0,
                        PixelFormat.Bgra,
                        PixelType.UnsignedByte,
                        data.Scan0);
                }

                GL.TexParameter(Type, TextureParameterName.TextureMinFilter,
                    (int) TextureMinFilter.Linear);
                GL.TexParameter(Type, TextureParameterName.TextureMagFilter,
                    (int) TextureMagFilter.Linear);

                GL.TexParameter(Type, TextureParameterName.TextureWrapS,
                    (int) TextureWrapMode.Repeat);
                GL.TexParameter(Type, TextureParameterName.TextureWrapT,
                    (int) TextureWrapMode.Repeat);

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            });
        }

        public Texture(int glHandle, TextureTarget type = TextureTarget.Texture2D)
        {
            Handle = glHandle;
            Type = type;
        }

        public Texture(TextureTarget type = TextureTarget.Texture2D) : this(GL.GenTexture(), type)
        {
        }

        public static Texture CreateMultiSampled(Vector2i size, int samples,
            PixelInternalFormat format = PixelInternalFormat.Rgb16f)
        {
            var texture = new Texture(TextureTarget.Texture2DMultisample)
            {
                Size = default
            };
            texture.Size = size;
            texture.RunWithBinding(() =>
            {
                GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, samples, format,
                    size.X,
                    size.Y, true);
                GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureMinFilter,
                    (int) TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureMagFilter,
                    (int) TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureWrapS,
                    (int) TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureWrapT,
                    (int) TextureWrapMode.ClampToEdge);
            });
            return texture;
        }

        public static Texture Create(Vector2i size, PixelInternalFormat internalFormat = PixelInternalFormat.Rgb16f,
            PixelFormat format = PixelFormat.Rgba, PixelType type = PixelType.Float)
        {
            var texture = new Texture
            {
                Size = size
            };
            texture.RunWithBinding(() =>
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, size.X,
                    size.Y, 0, format, type, (IntPtr) null);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int) TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int) TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                    (int) TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                    (int) TextureWrapMode.ClampToEdge);
            });
            return texture;
        }

        ~Texture() => Cleanup();

        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            Bind();
        }

        public void Disable() => Unbind();

        public override void Bind()
        {
            GL.BindTexture(Type, Handle);
        }

        public override void Unbind()
        {
            GL.BindTexture(Type, 0);
        }

        public void Dispose()
        {
            Cleanup();

            GC.SuppressFinalize(this);
        }

        private void Cleanup()
        {
            Unbind();

            GL.DeleteTexture(Handle);
        }
    }
}