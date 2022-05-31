using CourseWork.Common.Geometry;
using OpenTK.Mathematics;

namespace CourseWork.Common.Camera
{
    public class CameraOperator : IUpdateable
    {
        private const float E = 0.001f;
        private const float RotationSpeed = 3f;
        private const float MoveSpeed = 10f;

        public Camera Camera { get; set; }
        public Vector3 FollowOffset { get; set; }
        public Vector3 FollowRotation { get; set; }

        public bool Disabled { get; set; }



        public CameraOperator(Camera camera)
        {
            Camera = camera;
        }

        public Transformable Follow { get; set; }


        public void Update(double dt)
        {
            if (Disabled) return;

            UpdateRotation(dt);
            UpdatePosition(dt);
        }

        private void UpdateRotation(double dt)
        {
            var target = -Follow.Rotation + FollowRotation;

            if ((target - Camera.Rotation).LengthSquared < E) return;

            var angelChanges = Linearize(Camera.Rotation, target, RotationSpeed * (float) dt);

            Camera.Rotate(angelChanges);
        }

        private void UpdatePosition(double dt)
        {
            var offset =
                (Matrix4.CreateTranslation(FollowOffset - Follow.Origin) *
                 GeometryHelper.CreateRotationMatrix(Follow.Rotation) * Matrix4.CreateTranslation(Follow.Origin))
                .ExtractTranslation();
            var target = -Follow.Position - offset;

            if ((target - Camera.Position).LengthSquared < E) return;

            var positionChanges = Linearize(Camera.Position, target, MoveSpeed * (float) dt);

            Camera.Move(positionChanges);
        }

        private Vector3 Linearize(Vector3 source, Vector3 target, float step)
        {
            return (target - source) * step;
        }
    }
}