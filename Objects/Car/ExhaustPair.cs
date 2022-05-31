using CourseWork.Common;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Car
{
    public class ExhaustPair : FigurePair<Exhaust>, IUpdateable
    {
        private float _intensity;

        public float Intensity
        {
            get => _intensity;
            set
            {
                _intensity = value;
                Left.Intensity = _intensity;
                Right.Intensity = _intensity;
            }
        }

        public ExhaustPair(Exhaust left, Exhaust right, Vector3 offset) : base(left, right, offset)
        {
        }

        public void Update(double dt)
        {
            Left.Update(dt);
            Right.Update(dt);
        }
    }
}