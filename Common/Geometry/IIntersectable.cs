using System;
using OpenTK.Mathematics;

namespace CourseWork.Common.Geometry
{
    public interface IIntersectable : ITransformable
    {
        Box3 Bounds { get; }
        IIntersectable Intersects(IIntersectable intersectable);
    }
}