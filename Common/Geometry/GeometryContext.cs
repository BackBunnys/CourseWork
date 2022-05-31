using CourseWork.Objects;

namespace CourseWork.Common.Geometry
{
    public class GeometryContext
    {
        private readonly IntersectionObserver _intersectionObserver = new();

        public void Register(Figure figure)
        {
            _intersectionObserver.Register(figure);
        }
    }
}