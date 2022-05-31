using OpenTK.Mathematics;

namespace CourseWork.Common.Geometry
{
    public struct Particle
    {
        public readonly Vector3 Position;
        public readonly float Size;
        public readonly Vector3 Velocity;
        public readonly float Lifetime;
        public readonly float Type;
        private Vector3 Padding;

        public Particle(Vector3 position, float size = 0.05f) : this()
        {
            Position = position;
            Size = size;
        }

        public Particle(Vector3 position, Vector3 velocity) : this(position)
        {
            Velocity = velocity;
        }
    }
}