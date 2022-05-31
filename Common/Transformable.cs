using System;
using CourseWork.Common.Geometry;
using OpenTK.Mathematics;
using Vector3 = OpenTK.Mathematics.Vector3;
using Vector4 = OpenTK.Mathematics.Vector4;

namespace CourseWork.Common
{
    public class Transformable : ITransformable
    {
        private Matrix4 _transformMatrix = Matrix4.Identity;
        protected bool NeedCalculation;

        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _size = Vector3.One;

        public Vector3 Origin = Vector3.Zero;

        public event Action Changed;

        public Matrix4 TransformMatrix
        {
            get
            {
                if (NeedCalculation) RecalculateMatrix();
                return _transformMatrix;
            }
            protected set => _transformMatrix = value;
        }

        public Vector3 Position
        {
            get => _position;
            set
            {
                if (value == Position) return;
                _position = value;
                OnChanged();
            }
        }

        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                if (value == Rotation) return;
                _rotation = value;// new Vector3(value.X % 360, value.Y % 360, value.Z % 360);
                OnChanged();
            }
        }

        public Vector3 Size
        {
            get => _size;
            set
            {
                if (value == Size) return;
                _size = value;
                OnChanged();
            }
        }

        public Matrix4 Inverse => TransformMatrix.Inverted();

        protected Transformable()
        {
            Changed += () => NeedCalculation = true;
        }

        public void Move(Vector3 translation)
        {
            Position += translation;
            NeedCalculation = true;
        }

        public virtual void AngleRelativeMove(Vector3 translation)
        {
            Move((GeometryHelper.CreateRotationMatrix(-Rotation) * new Vector4(translation, 0)).Xyz);
        }

        public void Rotate(Vector3 axisRotations)
        {
            Rotation += axisRotations;
            NeedCalculation = true;
        }

        public void Scale(Vector3 factors)
        {
            Size += factors;
            NeedCalculation = true;
        }

        public void Scale(float factor)
        {
            Scale(new Vector3(factor));
        }

        protected virtual void RecalculateMatrix()
        {
            TransformMatrix = GeometryHelper.CreateTransform(Position, Rotation, Size, Origin);

            NeedCalculation = false;
        }

        protected virtual void OnChanged()
        {
            Changed?.Invoke();
        }
    }
}