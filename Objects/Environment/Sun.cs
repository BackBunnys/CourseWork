using CourseWork.Common.Light;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Environment
{
    public class Sun : DirectionalLight
    {
        public Sun()
        {
            Color = new Vector3(244 / 255.0f, 168 / 255.0f, 113 / 255.0f);
            AmbientIntensity = 0.6f;
            DiffuseIntensity = 1.2f;
            Direction = new Vector3(-50, -5, 52) * 10000;
        }
    }
}