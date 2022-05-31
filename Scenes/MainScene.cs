using System;
using System.Collections.Generic;
using CourseWork.Common;
using CourseWork.Common.Camera;
using CourseWork.Common.Geometry;
using CourseWork.Common.Light;
using CourseWork.Common.OpenGL;
using CourseWork.Common.Physic;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using CourseWork.Common.Render.Post_Processing;
using CourseWork.Common.Render.Targets;
using CourseWork.Core.AssetManagement;
using CourseWork.Core.SceneManagement;
using CourseWork.Objects;
using CourseWork.Objects.Car;
using CourseWork.Objects.Environment;
using CourseWork.Objects.Garage;
using CourseWork.Objects.Garage.Gate;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Image = CourseWork.Objects.Image;

namespace CourseWork.Scenes
{
    internal class MainScene : Scene
    {
        private readonly GraphicConfiguration _configuration;

        private double _globalTime;

        private float _gamma = 0.7f;
        private ShaderProgram _toneMappingShader;

        private Skybox _skybox;
        private FlatTerrain _flatTerrain;
        private Car _car;
        private Garage _garage;
        private Sun _sun;
        private CameraOperator _operator;

        private IntersectionObserver _intersectionObserver;

        private LightContext _lightContext;

        private ShaderProgram _computeShader;

        private RenderTexture _renderTexture;
        private BloomEffect _bloomEffect;
        private Image _image;

        private Forest _forest;

        private Pointer _pointer;

        private PhysicContext physicContext = new();

        public MainScene(GraphicConfiguration configuration, RenderTarget renderTarget, RenderStates renderStates,
            KeyboardState keyboardState) : base(
            renderTarget, renderStates, keyboardState)
        {
            _configuration = configuration;
        }

        public override void Load()
        {
            LoadTextures();
            LoadShaders();

            _skybox = new Skybox(new[]
            {
                "./assets/textures/skybox/sky-right.bmp",
                "./assets/textures/skybox/sky-left.bmp",
                "./assets/textures/skybox/sky-top.bmp",
                "./assets/textures/skybox/sky-back.bmp",
                "./assets/textures/skybox/sky-front.bmp",
                "./assets/textures/skybox/sky-back.bmp",
            })
            {
                Shader = AssetManager.GetShader("skyboxShader")
            };


            ComputeShaderProgram computeShader = null;

            if (_configuration.ParticlesEnabled)
            {
                ShaderProgramBuilder builder = new();
                _computeShader = builder.Create()
                    .WithShader(ShaderType.ComputeShader, "./assets/shaders/particle-shader.comp")
                    .Build();
                computeShader = new ComputeShaderProgram(_computeShader.Handle)
                {
                    ComputeGroups = new Vector3i(1, 1, 1)
                };
            }

            _car = new Car(computeShader)
            {
                Position = new Vector3(0, 0, -5)
            };

            _garage = new Garage()
            {
                Rotation = new Vector3(180, 0, 0),
                Size = new Vector3(5, 3, 5),
                Position = new Vector3(0, 1.37f, 0)
            };

            _sun = new Sun();

            _flatTerrain = new FlatTerrain(new Vector2(25, 25))
            {
                Texture = AssetManager.GetTexture("grass"),
                Size = Vector3.One * 8
            };

            _operator = new CameraOperator(RenderTarget.Camera)
            {
                Follow = _car,
                FollowOffset = new Vector3(-4, 2, 0),
                FollowRotation = new Vector3(90, 15, 0)
            };

            _forest = new Forest(new Vector2i(100, 100), 1f, (_configuration.EnvironmentLevel + 1) * 2 + 1)
            {
                Size = Vector3.One * 2,
                Position = new Vector3(0, 0, 0)
            };

            _pointer = new Pointer()
            {
                Position = new Vector3(0, 6, 0)
            };

            _intersectionObserver = new IntersectionObserver();
            _intersectionObserver.Register(_garage);
            _intersectionObserver.Register(_car);

            _intersectionObserver.Intersected += (result) =>
            {
                Console.WriteLine("COLLISION");
                physicContext.ModelCollision(result.Target, result.With, result.Velocity);
            };

            Drawables.AddRange(new List<ITargetDrawable>
                {_skybox, _flatTerrain, _car, _forest, _garage, _pointer});

            _lightContext = new LightContext();
            _lightContext.DirectionalLights.Add(_sun);
            _lightContext.Combine(_garage.LightContext);
            _lightContext.Combine(_car.LightContext);

            _renderTexture = new RenderTexture(RenderTarget)
            {
                LightContext = _lightContext
            };
            _renderTexture.EnableMultiSampling((int) Math.Pow(2, _configuration.MsaaLevel));
            _renderTexture.EnableDepth();
            _renderTexture.RenderMode = _configuration.WireMode ? RenderMode.Lines : RenderMode.Fill;

            _image = new Image();
            _bloomEffect = new BloomEffect(RenderTarget.Size, (_configuration.PostProcessLevel + 1) * 2);

            _toneMappingShader = AssetManager.GetShader("toneMappingShader");
        }

        public override void Draw()
        {
            RenderTarget.Clear();

            _lightContext.Apply(RenderStates.Shader);
            _renderTexture.Clear();
            Drawables.ForEach(d => d.Draw(_renderTexture, RenderStates));
            _renderTexture.Display();

            _image.Texture = _bloomEffect.Apply(_renderTexture.Texture);
            _toneMappingShader.Use();
            _toneMappingShader.SetUniform("gamma", _gamma);
            _image.Draw(RenderTarget, RenderStates with {Shader = _toneMappingShader});
            RenderTarget.Display();
        }

        public override void Update(double dt)
        {
            _globalTime += dt;

            ProcessInput();
            physicContext.Update(dt);

            _garage.Update(dt);
            _car.Update(dt);
            _operator.Update(dt);

            _pointer.Update(dt);
        }

        private void ProcessInput()
        {
            if (KeyboardState.IsKeyDown(Keys.W))
                RenderTarget.Camera.AngleRelativeMove(new Vector3(0, 0, 0.1f));
            if (KeyboardState.IsKeyDown(Keys.S))
                RenderTarget.Camera.AngleRelativeMove(new Vector3(0, 0, -0.1f));
            if (KeyboardState.IsKeyDown(Keys.A))
                RenderTarget.Camera.AngleRelativeMove(new Vector3(0.1f, 0, 0));
            if (KeyboardState.IsKeyDown(Keys.D))
                RenderTarget.Camera.AngleRelativeMove(new Vector3(-0.1f, 0, 0));

            if (KeyboardState.IsKeyDown(Keys.Up))
                _car.MoveState = Car.IMoveState.Gas;
            if (KeyboardState.IsKeyDown(Keys.Down))
                _car.MoveState = Car.IMoveState.Braking;
            if (KeyboardState.IsKeyDown(Keys.Left))
                _car.Left();
            if (KeyboardState.IsKeyDown(Keys.Right))
                _car.Right();
        }

        public override void HandleKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.D1:
                    _garage.Gate = Garage.LiftingTurningGate;
                    break;
                case Keys.D2:
                    _garage.Gate = Garage.SwingGate;
                    break;
                case Keys.PageUp:
                    _gamma += 0.1f;
                    break;
                case Keys.PageDown:
                    _gamma -= 0.1f;
                    break;

                case Keys.F:
                    _operator.Disabled = !_operator.Disabled;
                    break;
            }
        }

        public override void HandleKeyUp(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Up or Keys.Down:
                    _car.MoveState = Car.IMoveState.Idle;
                    break;
                case Keys.Space:
                    if (_garage.Gate.State is GateState.Closed or GateState.Closing)
                        _garage.Gate.Open();
                    else
                        _garage.Gate.Close();
                    break;
                case Keys.G:
                    _operator.FollowRotation = new Vector3(180, 15, 0);
                    _operator.FollowOffset = new Vector3(0, 1, -1);
                    break;
                case Keys.L:
                    if (e.Control)
                        _car.ChangeLight(((Car.LightType) (((int) _car.Light + 1) % 3)));
                    else
                        _garage.ToggleLight();
                    break;
            }
        }

        public override void HandleInput(MouseMoveEventArgs e)
        {
            RenderTarget.Camera.Rotate(new Vector3(e.Delta));
        }

        public override void HandleInput(MouseWheelEventArgs e)
        {
            _operator.FollowOffset -= _operator.FollowOffset * 0.1f * e.OffsetY;
        }

        private void LoadTextures()
        {
            AssetManager.LoadTexture("wheel", "./assets/textures/oWheel.png", IStoringStrategy.PermanentStoring);
            AssetManager.LoadTextureSet("wall",
                new TextureSetInfo("./assets/textures/wall.jpg", "./assets/textures/wall-normal.jpg"),
                IStoringStrategy.PermanentStoring);
            AssetManager.LoadTexture("floor", "./assets/textures/floor.jpg");
            AssetManager.LoadTexture("gate", "./assets/textures/gate.jpg", IStoringStrategy.PermanentStoring);
            AssetManager.LoadTextureSet("shiver",
                new TextureSetInfo("./assets/textures/shiver.jpg", "./assets/textures/shiver-normal.jpg"),
                IStoringStrategy.PermanentStoring);
            AssetManager.LoadTextureSet("grass",
                new TextureSetInfo("./assets/textures/grass.jpg", "./assets/textures/grass-normal.jpg"));
            AssetManager.LoadTexture("wood", "./assets/textures/wood.jpg");
            AssetManager.LoadTexture("smoke", "./assets/textures/smoke_sprite.png", IStoringStrategy.PermanentStoring);
            AssetManager.LoadTextureSet("bark",
                new TextureSetInfo("./assets/textures/bark.jpg", "./assets/textures/bark-normal.jpg"));
        }

        private void LoadShaders()
        {
            AssetManager.LoadShader("lightShader",
                new ShaderProgramInfo("./assets/shaders/light-shader.vert", "./assets/shaders/light-shader.frag"),
                IStoringStrategy.PermanentStoring);
            AssetManager.LoadShader("skyboxShader", new ShaderProgramInfo("./assets/shaders/skybox-shader.vert",
                "./assets/shaders/skybox-shader.frag"));
            AssetManager.LoadShader("particleShader", new ShaderProgramInfo("./assets/shaders/particle-shader.vert",
                    "./assets/shaders/particle-shader.frag", "./assets/shaders/particle-shader.geom"),
                IStoringStrategy.PermanentStoring);
            AssetManager.LoadShader("imageShader", new ShaderProgramInfo("./assets/shaders/image-shader.vert",
                "./assets/shaders/image-shader.frag"));
            AssetManager.LoadShader("instancedShader", new ShaderProgramInfo("./assets/shaders/instanced-shader.vert",
                "./assets/shaders/shader.frag"));
            AssetManager.LoadShader("toneMappingShader", new ShaderProgramInfo("./assets/shaders/empty-shader.vert",
                "./assets/shaders/tone-mapping-shader.frag"));
            AssetManager.LoadShader("blurShader", new ShaderProgramInfo("./assets/shaders/empty-shader.vert",
                "./assets/shaders/blur-shader.frag"));
            AssetManager.LoadShader("brightnessShader", new ShaderProgramInfo("./assets/shaders/empty-shader.vert",
                "./assets/shaders/brightness-shader.frag"));
            AssetManager.LoadShader("combineShader", new ShaderProgramInfo("./assets/shaders/empty-shader.vert",
                "./assets/shaders/combine-shader.frag"));
        }
    }
}