using System;
using CourseWork.Common.OpenGL;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using CourseWork.Common.Render.Targets;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CourseWork.Common
{
    public class ParticleSystem : TargetDrawable<ParticleList>, IUpdateable
    {
        private Vector3 _globalPosition;
        private Vector3 _globalDirection;
        private float _globalSize;
        private Vector3 _velocity = Vector3.UnitY;
        private Vector3 _directionError = new(0.1f);
        private float _particleLifetime = 5;

        private float _globalTime;

        public ComputeShaderProgram ComputeShader { get; set; }
        public uint MaxParticles { get; private init; }

        public Vector3 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        public Vector3 DirectionError
        {
            get => _directionError;
            set => _directionError = value;
        }

        public float ParticleLifetime
        {
            get => _particleLifetime;
            set => _particleLifetime = value;
        }

        private readonly BufferObject _vbo;

        private int _spawnPointer;
        private int _spawnNewCount;


        public ParticleSystem(ComputeShaderProgram updateProgram, uint maxParticles) : base(
            new ParticleList(maxParticles))
        {
            ComputeShader = updateProgram;
            if (ComputeShader != null)
                ComputeShader.ComputeGroups =
                    new Vector3i((int) System.Math.Ceiling(maxParticles / (double) ComputeShader.WorkGroupSize.X), 1,
                        1);

            MaxParticles = maxParticles;
            _vbo = Drawable.Vbo;
        }

        public void Update(double dt)
        {
            if (ComputeShader == null) return;

            var deltaTime = (float) dt;
            _globalTime += deltaTime;

            PrepareComputing(deltaTime);

            _vbo.Type = BufferTarget.ShaderStorageBuffer;
            _vbo.Bind();
            _vbo.BindBase(BufferRangeTarget.ShaderStorageBuffer);
            ComputeShader.Compute();
            _vbo.Unbind();
            _vbo.Type = BufferTarget.ArrayBuffer;

            _spawnPointer = (int) ((_spawnPointer + _spawnNewCount) % MaxParticles);
            _spawnNewCount = 0;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (ComputeShader == null) return;

            ComputeShader.Wait();
            GL.DepthMask(false);
            base.Draw(target, states);
            GL.DepthMask(true);

            var globalTransform = TransformMatrix * states.Transform;
            _globalPosition = globalTransform.ExtractTranslation();
            _globalDirection = globalTransform.ExtractRotation() * Velocity;
            _globalSize = globalTransform.ExtractScale().LengthSquared;
        }

        public void Spawn(int count)
        {
            _spawnNewCount += count;
        }

        private void PrepareComputing(float deltaTime)
        {
            ComputeShader.Use();
            ComputeShader.SetUniform("globalTime", _globalTime);
            ComputeShader.SetUniform("deltaTime", deltaTime);
            ComputeShader.SetUniform("velocity", ref _globalDirection);
            ComputeShader.SetUniform("directionError", ref _directionError);
            ComputeShader.SetUniform("position", ref _globalPosition);
            ComputeShader.SetUniform("size", _globalSize);
            ComputeShader.SetUniform("spawnPointer", _spawnPointer);
            ComputeShader.SetUniform("spawnNew", _spawnNewCount);
            ComputeShader.SetUniform("lifetime", _particleLifetime);
        }
    }
}