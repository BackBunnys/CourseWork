using OpenTK.Mathematics;

namespace CourseWork.Common.Geometry
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector2 TextureCoords;
        public Vector3 Normal;
        public Vector3 Tangent;

        public Vertex(Vector3 position) : this()
        {
            Position = position;
        }

        public Vertex(Vector3 position, Vector2 textureCoords) : this(position)
        {
            TextureCoords = textureCoords;
        }

        public Vertex(Vector3 position, Vector3 normal) : this(position)
        {
            Normal = normal;
        }

        public Vertex(Vector3 position, Vector2 textureCoords, Vector3 normal) : this(position, textureCoords)
        {
            Normal = normal;
        }

        public Vertex(Vector3 position, Vector2 textureCoords, Vector3 normal, Vector3 tangent) : this(position, textureCoords, normal)
        {
            Tangent = tangent;
        }
    }
}