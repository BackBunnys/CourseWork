using System.Collections.Generic;
using System.Linq;
using CourseWork.Common;
using CourseWork.Common.Geometry;
using CourseWork.Common.Light;
using CourseWork.Common.Render;
using CourseWork.Objects.Car;
using CourseWork.Objects.Garage.Gate;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Garage
{
    public class Garage : Model, IUpdateable, ILightSource
    {
        public static readonly LiftingTurningGate LiftingTurningGate = new(new Vector3(0.8f, 0.75f, 0.05f))
            {Rotation = new Vector3(180, 0, 0), Position = new Vector3(0, -0.075f, 0.475f)};

        public static readonly SwingGate SwingGate = new(new Vector3(0.8f, 0.75f, 0.05f))
            {Rotation = new Vector3(180, 0, 0), Position = new Vector3(0, -0.075f, 0.475f)};

        private Gate.Gate _gate;

        public Gate.Gate Gate
        {
            get => _gate;
            set
            {
                if (_gate != null)
                {
                    _gate.Changed -= RecalculateLocalBounds;
                    Drawables.Remove(_gate);
                }

                _gate = value;
                _gate.Changed += RecalculateLocalBounds;
                Drawables.Add(_gate);
                RecalculateLocalBounds();
            }
        }

        public LightContext LightContext { get; }

        public bool IsLightOn => LightContext.Enabled;

        public Garage()
        {
            Gate = LiftingTurningGate;

            var lamps = new List<Lamp>(4);

            for (var i = -1; i < 2; ++i)
            {
                if (i == 0) continue;
                for (var j = -1; j < 2; ++j)
                {
                    if (j == 0) continue;
                    var lamp = new Lamp(new Vector3(0.2f, 0.025f, 0.2f))
                    {
                        Position = new Vector3(0.25f * i, 0.3f, 0.25f * j)
                    };
                    lamps.Add(lamp);
                }
            }

            var lights = lamps.Select(l => l.LightContext);

            Drawables.AddRange(new List<IDrawableGeometry>
            {
                new Wall(new Vector3(0.8f, 0.8f, 0.1f)) {Position = new Vector3(0, -0.1f, -0.45f)},
                new Wall(new Vector3(1, 0.8f, 0.1f))
                    {Rotation = new Vector3(90, 0, 0), Position = new Vector3(0.45f, -0.1f, 0)},
                new Wall(new Vector3(1, 0.8f, 0.1f))
                    {Rotation = new Vector3(90, 0, 0), Position = new Vector3(-0.45f, -0.1f, 0)},
                new Floor(new Vector3(0.9f, 0.8f, 0.05f))
                    {Rotation = new Vector3(0, 90, 90), Position = new Vector3(0, -0.475f, 0.05f)},
                new Shelf(new Vector3(0.7f, 0.05f, 0.1f))
                    {Rotation = new Vector3(-90, 0, 0), Position = new Vector3(0.35f, 0f, 0)},
                new Roof(new Vector3(1, 0.5f, 0.05f))
                    {Position = new Vector3(0, 0.55f, 0)}
            });
            Drawables.AddRange(lamps);

            LightContext = new LightContext();
            LightContext.Combine(lights);
            InitBounds();
        }

        public void LightOn() => LightContext.Enable();

        public void LightOff() => LightContext.Disable();

        public void ToggleLight()
        {
            if (IsLightOn)
                LightContext.Disable();
            else
                LightContext.Enable();
        }

        public void Update(double dt)
        {
            Gate.Update(dt);
        }
    }
}