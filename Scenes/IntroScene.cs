using System;
using System.Numerics;
using CourseWork.Common;
using CourseWork.Common.Camera;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Targets;
using CourseWork.Core.AssetManagement;
using CourseWork.Core.SceneManagement;
using CourseWork.Objects;
using NAudio.Wave;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Image = CourseWork.Objects.Image;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace CourseWork.Scenes
{
    internal class IntroScene : Scene
    {
        private WaveOutEvent _outputDevice;
        private AudioFileReader _audioFile;
        private Image _image;

        private double _duration;

        private double _elapsedTime;


        public IntroScene(RenderWindow renderTarget, RenderStates renderStates, KeyboardState keyboardState) : base(
            new RenderWindow(renderTarget.Window, new Camera(Vector3.Zero),
                Matrix4.CreateOrthographic(renderTarget.Size.X, renderTarget.Size.Y, 0.1f, 100f), renderTarget.Size),
            renderStates, keyboardState)
        {
        }

        public override void Load()
        {
            AssetManager.LoadTexture("logo", "./assets/images/logo.png");
            AssetManager.LoadShader("carShader",new ShaderProgramInfo("./assets/shaders/car-shader.vert",
                "./assets/shaders/car-shader.frag"));
            AssetManager.LoadSound("launchSound", "./assets/sounds/GTR_Launch.mp3");

            _image = new Image(AssetManager.GetTexture("logo"))
            {
                Shader = AssetManager.GetShader("carShader"),
                Size = new Vector3(RenderTarget.Size.Y / 4f),
                Position = -Vector3.UnitZ,
                Color = Color4.Black
            };

            _outputDevice = new WaveOutEvent();
            _audioFile = AssetManager.GetSound("launchSound");
            _audioFile.Volume = 0.1f;
            _outputDevice.Init(_audioFile);

            _duration = _audioFile.TotalTime.TotalSeconds;

            _outputDevice.Play();
        }

        public override void Unload()
        {
            _outputDevice.Dispose();
            _audioFile.Dispose();
        }

        public override void Draw()
        {
            RenderTarget.Clear(_image.Color);
            _image.Draw(RenderTarget, RenderStates);
            RenderTarget.Display();
        }

        public override void Update(double dt)
        {
            _elapsedTime += dt;

            var normalizedDeltaTime = (float)(_elapsedTime / _duration);
            var sinDeltaTime = (float)Math.Sin(normalizedDeltaTime * Math.PI);
            _image.Color = new Color4(sinDeltaTime, sinDeltaTime, sinDeltaTime, 1);

            Finished = _outputDevice.PlaybackState == PlaybackState.Stopped;
        }
    }
}