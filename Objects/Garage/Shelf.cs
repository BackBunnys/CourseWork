using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Garage
{
    class Shelf : Box
    {
        public Shelf(Vector3 size) : base(size)
        { 
            Texture = AssetManager.GetTexture("wood");
        }
    }
}
