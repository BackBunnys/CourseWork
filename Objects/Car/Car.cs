using System;
using System.Collections.Generic;
using CourseWork.Common;
using CourseWork.Common.Light;
using CourseWork.Common.OpenGL;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Targets;
using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Car
{
    public class Car : Model, ILightSource
    {
        private const float E = 0.1f;

        public enum LightType
        {
            None,
            Dipped,
            High
        }

        public interface IMoveState
        {
            static readonly IMoveState Gas = new GasMoveState();
            static readonly IMoveState Braking = new BrakeMoveState();
            static readonly IMoveState Idle = new IdleMoveState();

            float NewSpeed(float speed);
        }

        private class GasMoveState : IMoveState
        {
            private const float GasCoefficient = 0.05f;
            private const float Limit = 100;

            public float NewSpeed(float speed)
            {
                if (speed > Limit) return Limit;

                return speed + GasCoefficient;
            }
        }

        private class BrakeMoveState : IMoveState
        {
            private const float BrakeCoefficient = 0.2f;
            private const float Limit = -10;

            public float NewSpeed(float speed)
            {
                if (speed < Limit) return Limit;

                return speed - BrakeCoefficient;
            }
        }

        private class IdleMoveState : IMoveState
        {
            private const float IdleCoefficient = 0.01f;

            public float NewSpeed(float speed)
            {
                return speed switch
                {
                    > IdleCoefficient => speed - IdleCoefficient,
                    < IdleCoefficient => speed + IdleCoefficient,
                    _ => 0
                };
            }
        }

        private float _speed;
        public IMoveState MoveState { get; set; } = IMoveState.Idle;
        public LightContext LightContext { get; } = new();
        public LightType Light { get; private set; } = LightType.High;
        public bool IsTurning { get; set; }

        private float _angle;
        private readonly float _wheelsDistance;

        private readonly IList<Wheel> _wheels;

        private readonly ExhaustPair _innerExhausts;
        private readonly ExhaustPair _outerExhausts;

        private readonly LightPair _headLights;
        private readonly LightPair _stopLights;

        public Car(ComputeShaderProgram computeShader)
        {
            _wheels = new[]
            {
                //front wheels
                new Wheel() {Position = new Vector3(1, 0.2f, -0.5f), Texture = AssetManager.GetTexture("wheel")},
                new Wheel() {Position = new Vector3(1, 0.2f, 0.5f), Texture = AssetManager.GetTexture("wheel")},
                //back wheels
                new Wheel() {Position = new Vector3(-1, 0.2f, -0.5f), Texture = AssetManager.GetTexture("wheel")},
                new Wheel() {Position = new Vector3(-1, 0.2f, 0.5f), Texture = AssetManager.GetTexture("wheel")},
            };

            _wheelsDistance = 2;
            Origin = new Vector3(-1, 0, 0);

            var body = new Body()
            {
                Size = new Vector3(1.4f, 1.4f, 0.5f),
                Position = new Vector3(0, 0.2f, 0),
                Rotation = new Vector3(180, 0, 0),
            };

            _innerExhausts = new ExhaustPair(new Exhaust(computeShader), new Exhaust(computeShader),
                new Vector3(0, 0, 0.2f))
            {
                Position = new Vector3(-0.45f, 0.165f, 0)
            };
            _outerExhausts = new ExhaustPair(new Exhaust(computeShader), new Exhaust(computeShader),
                new Vector3(0, 0, 0.275f))
            {
                Position = new Vector3(-0.45f, 0.165f, 0)
            };

            _headLights = new LightPair(new Light(new Vector3(0.1f)), new Light(new Vector3(0.1f)),
                new Vector3(0, 0, 0.3f))
            {
                Position = new Vector3(1.35f, 0.40f, 0),
                Color = Color4.White with {A = 0.5f}
            };

            _stopLights = new LightPair(new Light(new Vector3(0.1f)), new Light(new Vector3(0.1f)),
                new Vector3(0, 0, 0.35f))
            {
                Position = new Vector3(-1.4f, 0.4f, 0),
                Rotation = new Vector3(180, 0, 0),
                Color = Color4.DarkRed with {A = 0.5f}
            };

            _stopLights.LightContext.Disable();
            LightContext.Combine(new[] {_headLights.LightContext, _stopLights.LightContext});

            Drawables.Add(body);
            Drawables.AddRange(_wheels);

            InitBounds();
        }

        public void Update(double dt)
        {
            _speed = MoveState.NewSpeed(_speed);
            var translation = _speed * dt;
            if (Math.Abs(_angle) > E && Math.Abs(_speed) > E)
            {
                var turnRadius =
                    _wheelsDistance / MathHelper.DegreesToRadians(_angle) / 2; //R = D * ctg(a) / 2
                if (Math.Abs(_speed) > 1)
                    turnRadius *= _speed;

                var turnAngle = 180 * translation / (Math.PI * turnRadius); //a = 180*L / PI*R

                Rotate(new Vector3((float) (_speed > 0 ? turnAngle : -turnAngle), 0, 0));
            }

            UpdateAngle();
            IsTurning = false;

            if (Math.Abs(_speed) > E)
            {
                AngleRelativeMove(new Vector3((float) translation, 0, 0));
                RotateWheels((float) (-360 * translation / (Math.PI * _wheels[0].Size.Y)));
            }

            UpdateWheels();
            UpdateLights();
            UpdateExhausts(dt);
        }

        private void UpdateAngle()
        {
            if (IsTurning || Math.Abs(_angle) < E) return;

            var change = _angle / Math.Abs(_angle);
            if (Math.Abs(_angle) - Math.Abs(change) < 0)
                _angle = 0;
            else
                _angle -= change;
            
        }

        private void RotateWheels(float rotation)
        {
            var wheelRotation = new Vector3(0, 0, rotation);
            foreach (var wheel in _wheels)
            {
                wheel.Rotate(wheelRotation);
            }
        }

        private void UpdateWheels()
        {
            _wheels[0].Rotation = new Vector3(_angle, 0, _wheels[0].Rotation.Z);
            _wheels[1].Rotation = new Vector3(_angle, 0, _wheels[0].Rotation.Z);
        }

        private void UpdateLights()
        {
            if (MoveState != IMoveState.Braking)
            {
                _stopLights.Disable();
            }
            else
            {
                _stopLights.Enable();
            }
        }

        private void UpdateExhausts(double dt)
        {
            _outerExhausts.Intensity = 10 + 10 * _speed;
            _innerExhausts.Intensity = 10 + 10 * _speed;

            _outerExhausts.Update(dt);
            _innerExhausts.Update(dt);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);

            var localStates = states with {Transform = TransformMatrix * states.Transform};

            _outerExhausts.Draw(target, localStates);
            _innerExhausts.Draw(target, localStates);
            _headLights.Draw(target, localStates);
            _stopLights.Draw(target, localStates);
        }

        public void ChangeLight(LightType type)
        {
            Light = type;
            switch (type)
            {
                case LightType.None:
                    _headLights.Disable();
                    break;
                case LightType.Dipped:
                case LightType.High:
                default:
                    _headLights.Enable();
                    _headLights.Intensity = (int) type / 2.0f;
                    break;
            }
        }

        public void Left()
        {
            IsTurning = true;
            if (_angle < 40)
                _angle += 5f;
        }

        public void Right()
        {
            IsTurning = true;
            if (_angle > -40)
                _angle -= 5f;
        }
    }
}