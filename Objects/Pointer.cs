using System;
using CourseWork.Common;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class Pointer : Model, IUpdateable
    {
        private double _globalTime;

        private readonly Arrow _arrow;

        public Vector3 TranslationFactors { get; set; } = Vector3.UnitY;
        public float TranslationSpeed { get; set; } = 2;
        public Vector3 RotationAngles { get; set; } = new Vector3(45, 0, 0);
        public float RotationSpeed { get; set; } = 1.5f;

        public Pointer()
        {
            _arrow = new Arrow()
            {
                Color = Color4.White
            };

            Drawables.Add(_arrow);
        }

        public void Update(double dt)
        {
            _globalTime += dt;

            _arrow.Position = TranslationFactors * (float) Math.Sin(_globalTime * TranslationSpeed);
            _arrow.Rotation = RotationAngles * (float) Math.Sin(_globalTime * RotationSpeed);
        }
    }
}