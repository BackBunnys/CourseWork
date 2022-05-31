using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;

namespace CourseWork.Common.Math
{
    public static class MathHelper
    {
        public static bool Less(this Vector3 v1, Vector3 v2)
        {
            return v1.Z <= v2.Z && v1.Y <= v2.Y && v1.X <= v2.X;
        }

        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return v1.Less(v2) ? v1 : v2;
        }

        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return v1.Less(v2) ? v2 : v1;
        }

        public static List<float> Raw(this Vector3 point)
        {
            return new List<float> {point.X, point.Y, point.Z};
        }

        public static List<float> Raw(this IEnumerable<Vector3> points)
        {
            return points.Select(p => p.Raw()).Aggregate((accumulator, rawPoint) =>
            {
                accumulator.AddRange(rawPoint);
                return accumulator;
            });
        }

        public static List<float> Raw(this Vector4 point)
        {
            return new List<float> {point.X, point.Y, point.Z, point.W};
        }

        public static List<float> Raw(this IEnumerable<Vector4> points)
        {
            return points.Select(p => p.Raw()).Aggregate((accumulator, rawPoint) =>
            {
                accumulator.AddRange(rawPoint);
                return accumulator;
            });
        }

        public static List<float> Raw(this IEnumerable<Matrix4> matrices)
        {
            return matrices.Select(m => new[] {m.Column0, m.Column1, m.Column2, m.Column3}.Raw()).Aggregate(
                (accumulator, rawPoint) =>
                {
                    accumulator.AddRange(rawPoint);
                    return accumulator;
                });
        }
    }
}