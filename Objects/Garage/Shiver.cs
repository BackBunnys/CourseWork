using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Garage
{
    class Shiver : Box
    {
        public Shiver(Vector3 size) : base(size)
        {
            Texture = AssetManager.GetTexture("shiver");
            TextureTiling = size * 3;
        }
    }
}
