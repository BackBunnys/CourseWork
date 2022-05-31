using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace CourseWork.Common.Render.Targets
{
    class RenderWindow : RenderTarget
    {
        public GameWindow Window { get; }

        public RenderWindow(GameWindow window, Camera.Camera camera, Matrix4 projection, Vector2i size) : base(camera, projection, size)
        {
            Window = window;
        }

        public override void Display()
        {
            Window.SwapBuffers();
        }
    }
}