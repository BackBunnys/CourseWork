using CourseWork.Common.Light;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Car
{
    public class LightPair : FigurePair<Light>, ILightSource
    {
        private Color4 _color;
        private float _intensity;

        public Color4 Color
        {
            get => _color;
            set
            {
                _color = value;
                Left.Color = value;
                Right.Color = value;
            }
        }

        public float Intensity
        {
            get => _intensity;
            set
            {
                _intensity = value;
                Left.Intensity = value;
                Right.Intensity = value;
            }
        }

        public void Enable() => LightContext.Enable();
        public void Disable() => LightContext.Disable();

        public LightContext LightContext { get; } = new();

        public LightPair(Light leftLight, Light rightLight, Vector3 offset) : base(leftLight, rightLight, offset)
        {
            LightContext.Combine(new[] {Left.LightContext, Right.LightContext});
        }
    }
}