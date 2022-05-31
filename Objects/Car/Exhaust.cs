using System.Collections.Generic;
using System.Linq;
using CourseWork.Common;
using CourseWork.Common.Geometry;
using CourseWork.Common.OpenGL;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Targets;
using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Car
{
    public class Exhaust : RotationBody, IUpdateable
    {
        private static readonly IEnumerable<Vector2> Generatrix;

        private readonly ParticleSystem _particleSystem;

        private float _spawnNew;

        public float Intensity { get; set; } = 10;

        static Exhaust()
        {
            var generatrix = ErmitQurve.CalculatePoints(new List<KeyValuePair<Vector3, Vector3>>()
            {
                new(new Vector3(0 - 30f, 1.2f, 0), -Vector3.Zero),
                new(new Vector3(0.0f - 30f, 1.5f, 0), Vector3.UnitX / 2),
                new(new Vector3(0.4f - 30f, 1.2f, 0), Vector3.UnitX),
                new(new Vector3(2 - 30f, 1.2f, 0), Vector3.UnitX),
                new(new Vector3(2 - 30f, 2, 0), Vector3.UnitX),
                new(new Vector3(10 - 30f, 2, 0), Vector3.UnitX),
                new(new Vector3(10 - 30f, 1.3f, 0), Vector3.UnitX / 2),
                new(new Vector3(13 - 30f, 1.3f, 0), Vector3.Zero)
            }, 10).ToList();
            generatrix.Add(new Vector2(30, 1.2f));
            Generatrix = GeometryHelper.Normalize(generatrix);
        }

        public Exhaust(ComputeShaderProgram computeShader) : base(Generatrix, 36)
        {
            Size = new Vector3(1f, 0.035f, 0.035f);
            Rotation = new Vector3(0, 0, 0);
            Color = Color4.Silver;
            _particleSystem = new ParticleSystem(computeShader, 1000)
            {
                Shader = AssetManager.GetShader("particleShader"),
                Texture = AssetManager.GetTexture("smoke"),
                ParticleLifetime = 5,
                Velocity = -Vector3.UnitZ / 10,
                Rotation = new Vector3(90, 0, 0),
                DirectionError = new Vector3(0.05f),
                Color = Color4.White with {A = 0f},
                ColorMixing = 0.9f,
                Size = Vector3.One / 2,
                Position = -Size
            };
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            _particleSystem.Draw(target, states with {Transform = TransformMatrix * states.Transform});
        }

        public void Update(double dt)
        {
            _spawnNew += Intensity * (float) dt;

            _particleSystem.Velocity = -Vector3.UnitZ / 20 * Intensity;
            _particleSystem.ParticleLifetime = 5 * 10 / Intensity;
            _particleSystem.Spawn((int) _spawnNew);
            _particleSystem.Update(dt);

            _spawnNew -= (int) _spawnNew;
        }
    }
}