using CourseWork.Common;
using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace CourseWork.Objects.Garage
{
    public class Wall : Box
    {

        public Wall(Vector3 size) : base(size)
        {
            Texture = AssetManager.GetTexture("wall");
            TextureTiling = size;
        }
    }
}