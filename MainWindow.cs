using CourseWork.Core.AssetManagement;
using CourseWork.Core.SceneManagement;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using CourseWork.Common;
using CourseWork.Common.Camera;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Targets;
using CourseWork.Scenes;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace CourseWork
{
    public class GraphicConfiguration
    {
        public int MsaaLevel = 2;
        public uint EnvironmentLevel = 1;
        public uint PostProcessLevel = 1;
        public bool WireMode = false;
        public bool ParticlesEnabled = true;
    }

    public class MainWindow : GameWindow
    {
        private readonly GraphicConfiguration _graphicConfiguration;
        private SceneOrchestrator _sceneOrchestrator;
        private RenderTarget _renderTarget;
        private RenderStates _renderStates;

        private int _fpsCounter;
        private double _fpsTime;

        public MainWindow(GraphicConfiguration graphic, GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings) : base(
            gameWindowSettings, nativeWindowSettings)
        {
            _graphicConfiguration = graphic;
            Console.WriteLine(GL.GetString(StringName.Version));
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            AssetManager.LoadShader("defaultShader",
                new ShaderProgramInfo("./assets/shaders/shader.vert", "./assets/shaders/shader.frag"),
                IStoringStrategy.PermanentStoring);
            AssetManager.LoadShader("normalShader", new ShaderProgramInfo("./assets/shaders/normal-shader.vert",
                    "./assets/shaders/normal-shader.frag", "./assets/shaders/normal-shader.geom"),
                IStoringStrategy.PermanentStoring);

            GL.ClearColor(Color4.White);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Multisample);

            _renderTarget = new RenderWindow(this,
                new Camera(Vector3.Zero),
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60),
                    Size.X / (float) Size.Y, 0.1f, 100f),
                Size);

            _renderStates = new RenderStates
            {
                Shader = AssetManager.GetShader("defaultShader")
            };

            CursorGrabbed = true;
            _sceneOrchestrator = new SceneOrchestrator(new List<Scene>()
            {
                new IntroScene((RenderWindow)_renderTarget, _renderStates, KeyboardState),
                new MainScene(_graphicConfiguration, _renderTarget, _renderStates, KeyboardState),
            });
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            _sceneOrchestrator.CurrentScene.Draw();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            ProcessFps(args.Time);

            Console.WriteLine(
                $"Draw calls: {Statistics.DrawCalls}\nVertices: {Statistics.Vertices}\nIndices: {Statistics.Indices}\n\n");
            Statistics.Clear();

            _sceneOrchestrator.CurrentScene.Update(args.Time);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Keys.Escape:
                    Close();
                    break;
                default:
                    _sceneOrchestrator.CurrentScene.HandleKeyDown(e);
                    break;
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            _sceneOrchestrator.CurrentScene.HandleInput(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            _sceneOrchestrator.CurrentScene.HandleKeyUp(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _sceneOrchestrator.CurrentScene.HandleInput(e);
        }

        private void ProcessFps(double dt)
        {
            _fpsTime += dt;
            ++_fpsCounter;

            if (_fpsTime >= 1.0)
            {
                Console.WriteLine(_fpsCounter);
                _fpsTime = 0;
                _fpsCounter = 0;
            }
        }
    }
}