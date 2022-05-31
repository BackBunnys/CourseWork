using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Garage
{
    class Floor : Box
    {
        public Floor(Vector3 size) : base(size)
        {
            Texture = AssetManager.GetTexture("floor");
            TextureTiling = size;
        }
    }
}
