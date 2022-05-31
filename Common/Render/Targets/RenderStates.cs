using CourseWork.Common.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common.Render.Targets
{
    public struct RenderStates
    {
        public static readonly RenderStates Empty = new();

        private Matrix4? _transform;
        private Vector3? _textureTiling;

        public ShaderProgram Shader { get; init; }
        public Texture Texture { get; init; }

        public Vector3 TextureTiling
        {
            get => _textureTiling ??= Vector3.One;
            init => _textureTiling = value;
        }

        public Color4 Color { get; init; }
        public float ColorMixing { get; init; }

        public Matrix4 Transform
        {
            get => _transform ?? Matrix4.Identity;
            init => _transform = value;
        }

        public uint NumberOfInstances { get; init; }
        public Matrix4[] Transforms { get; init; }

        public RenderStates Combine(RenderStates states)
        {
            return this with
            {
                Transform = Transform * states.Transform,
                Texture = Texture ?? states.Texture,
                TextureTiling = TextureTiling,
                Shader = Shader ?? states.Shader,
                Color = Color,
                ColorMixing = Texture != null ? ColorMixing : 1f,
                NumberOfInstances = NumberOfInstances * states.NumberOfInstances
            };
        }
    }
}