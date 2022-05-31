namespace CourseWork.Core.AssetManagement
{
    public class TextureSetInfo
    {
        public readonly string ColorPath;
        public readonly string NormalPath;

        public TextureSetInfo(string colorPath, string normalPath = null)
        {
            ColorPath = colorPath;
            NormalPath = normalPath;
        }
    }
}