using System;
using CourseWork.Common;
using CourseWork.Common.Render;
using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Garage.Gate
{
    public class LiftingTurningGate : Gate
    {
        private readonly Box _gateBox;

        public LiftingTurningGate(Vector3 size)
        {
            _gateBox = new Box(size)
            {
                Origin = new Vector3(0, size.Y / 2, -size.Z / 2)
            };
            _gateBox.Texture = AssetManager.GetTexture("gate");

            Drawables.Add(_gateBox);
            InitBounds();
        }

        public override void Update(double dt)
        {
            base.Update(dt);
            _gateBox.Rotation = new Vector3(0, 90 / 100f * OpenPercentage, 0);
        }
    }
}