using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Garage.Gate
{
    public class SwingGate : Gate
    {
        private readonly Box _leftGateBox;
        private readonly Box _rightGateBox;

        public SwingGate(Vector3 size)
        {
            _leftGateBox = new Box(size with {X = size.X / 2})
            {
                Origin = new Vector3(-size.X / 4, 0, 0),
                Position = new Vector3(-size.X / 4, 0, 0),
                Texture = AssetManager.GetTexture("gate"),
                TextureTiling = new Vector3(0.5f, 1, 1)
            };
            _rightGateBox = new Box(size with {X = size.X / 2})
            {
                Origin = new Vector3(size.X / 4, 0, 0),
                Position = new Vector3(size.X / 4, 0, 0),
                Texture = AssetManager.GetTexture("gate"),
                TextureTiling = new Vector3(0.5f, 1, 1)
            };

            Drawables.Add(_leftGateBox);
            Drawables.Add(_rightGateBox);
            InitBounds();
        }

        public override void Update(double dt)
        {
            base.Update(dt);
            _leftGateBox.Rotation = new Vector3(90 / 100f * OpenPercentage, 0, 0);
            _rightGateBox.Rotation = new Vector3(-90 / 100f * OpenPercentage, 0, 0);
        }
    }
}