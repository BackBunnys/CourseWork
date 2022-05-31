using System;
using CourseWork.Common.Geometry;
using OpenTK.Mathematics;

namespace CourseWork.Common
{
    public interface ITransformable
    {
        public event Action Changed;

        public Matrix4 TransformMatrix { get; }

        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }

        public Vector3 Size { get; set; }

        public Matrix4 Inverse { get; }

        public void Move(Vector3 translation);

        public void AngleRelativeMove(Vector3 translation);

        public void Rotate(Vector3 axisRotations);

        public void Scale(Vector3 factors);

        public void Scale(float factor);
    }
}