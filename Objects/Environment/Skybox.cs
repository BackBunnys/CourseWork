using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using CourseWork.Common;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using CourseWork.Common.Render.Targets;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace CourseWork.Objects.Environment
{
    internal class Skybox : Figure
    {
        private static readonly Vertex[] Vertices =
        {
            new(new Vector3(-1.0f, 1.0f, -1.0f)),
            new(new Vector3(-1.0f, -1.0f, -1.0f)),
            new(new Vector3(1.0f, -1.0f, -1.0f)),
            new(new Vector3(1.0f, 1.0f, -1.0f)),

            new(new Vector3(-1.0f, 1.0f, 1.0f)),
            new(new Vector3(-1.0f, -1.0f, 1.0f)),
            new(new Vector3(1.0f, -1.0f, 1.0f)),
            new(new Vector3(1.0f, 1.0f, 1.0f)),
        };

        private static readonly uint[] Indices =
        {
            0, 1, 2,
            0, 2, 3,

            0, 4, 5,
            0, 5, 1,

            1, 5, 6,
            1, 6, 2,

            2, 6, 7,
            2, 7, 3,

            3, 7, 4,
            3, 4, 0,

            6, 5, 4,
            7, 6, 4,
        };

        public Skybox(ICollection<string> sidesPaths) : base(new Mesh(Vertices, Indices))
        {
            Texture = new Texture(TextureTarget.TextureCubeMap);
            LoadCubeMap(sidesPaths);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            GL.DepthFunc(DepthFunction.Lequal);
            GL.DepthMask(false);
            base.Draw(target, states);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
        }

        private void LoadCubeMap(ICollection<string> sidesPaths)
        {
            Texture.Bind();

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR,
                (int) TextureWrapMode.ClampToEdge);

            for (var i = 0; i < sidesPaths.Count; ++i)
                if (sidesPaths.ElementAt(i) != null)
                    LoadSide(sidesPaths.ElementAt(i), i);

            Texture.Unbind();
        }

        private static void LoadSide(string path, int number)
        {
            using var image = new Bitmap(path);

            var data = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + number,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);
        }
    }
}