using CourseWork.Common.Geometry;
using OpenTK.Mathematics;

namespace CourseWork.Common.Camera
{
    public class Camera : Transformable
    {
        public Camera()
        {
        }

        public Camera(Vector3 position)
        {
            Position = position;
        }

        public override void AngleRelativeMove(Vector3 translation)
        {
            Move((GeometryHelper.CreateRotationMatrix(Rotation) * new Vector4(translation, 0)).Xyz);
        }

        public static Camera LookedAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            return new Camera(eye)
            {
                Rotation = Matrix4.LookAt(eye, target, up).ExtractRotation().Xyz
            };
        }

        protected override void RecalculateMatrix()
        {
            TransformMatrix = Matrix4.CreateTranslation(Position) *
                              Matrix4.CreateTranslation(-Origin) *
                              GeometryHelper.CreateRotationMatrix(Rotation) *
                              Matrix4.CreateTranslation(Origin) *
                              Matrix4.CreateScale(Size);
            NeedCalculation = false;
        }
    }
}