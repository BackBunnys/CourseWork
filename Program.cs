using OpenTK.Windowing.Desktop;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTK.Windowing.Common;

namespace CourseWork
{
    public class Configuration
    {
        public bool FullScreen = true;
        public bool VSync = true;
        public GraphicConfiguration Graphic = new();
    }


    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Configuration configuration = new Configuration();

            AllocConsole();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartWindow(configuration));

            var gws = GameWindowSettings.Default;
            gws.UpdateFrequency = 120;
            gws.RenderFrequency = 120;


            var nws = NativeWindowSettings.Default;
            nws.Title = "Best GTR Garage";
            if (configuration.FullScreen)
            {
                nws.WindowState = WindowState.Fullscreen;
            }

            nws.Flags = ContextFlags.ForwardCompatible;
            nws.DepthBits = 24;
            nws.StencilBits = 8;
            nws.APIVersion = new Version(3, 3);

            MainWindow mainWindow = new(configuration.Graphic, gws, nws);

            mainWindow.VSync = configuration.VSync ? VSyncMode.On : VSyncMode.Adaptive;
            
            mainWindow.Run();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();
    }
}
