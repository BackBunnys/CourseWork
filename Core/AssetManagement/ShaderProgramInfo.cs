namespace CourseWork.Core.AssetManagement
{
    public struct ShaderProgramInfo
    {
        public readonly string VertexPath;
        public readonly string FragmentPath;
        public readonly string GeometryPath;

        public ShaderProgramInfo(string vertexPath, string fragmentPath, string geometryPath = null)
        {
            VertexPath = vertexPath;
            FragmentPath = fragmentPath;
            GeometryPath = geometryPath;
        }
    }
}