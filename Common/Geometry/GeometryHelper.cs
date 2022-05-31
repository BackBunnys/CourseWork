using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using Vector2 = OpenTK.Mathematics.Vector2;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace CourseWork.Common.Geometry
{
    public static class GeometryHelper
    {
        public static GeometryData Extrude(IList<Vector2> canvasPoints, bool capped = true, uint sidesPropagation = 0)
        {
            var pointsCount = (uint) canvasPoints.Count;

            var pointSum = canvasPoints.Aggregate((p, sum) => sum + p);
            var middlePoint = pointSum / pointsCount;


            var vertices = new Vertex[(int) ((pointsCount + 1) * (sidesPropagation + 2))];
            for (var i = 0; i <= sidesPropagation + 1; ++i)
            {
                vertices[i * (pointsCount + 1)] =
                    new Vertex(new Vector3(middlePoint) {Z = -1 + 2.0f / (sidesPropagation + 1) * i}, Vector2.One / 2);
            }

            var indices = new List<uint>();
            for (var i = 1; i <= pointsCount; ++i)
            {
                var point = canvasPoints[i - 1];
                var texturePoint = (Vector2.One + point.Normalized()) / 2;

                for (var j = 0; j <= sidesPropagation + 1; ++j)
                {
                    vertices[i + j * (pointsCount + 1)] =
                        new Vertex(new Vector3(point) {Z = -1 + 2.0f / (sidesPropagation + 1) * j}, texturePoint);
                }
            }

            for (var i = 0u; i <= sidesPropagation; ++i)
            {
                for (var j = 1u; j < pointsCount; ++j)
                {
                    indices.AddRange(Triangulate(new[]
                    {
                        j + i * (pointsCount + 1), j + (i + 1) * (pointsCount + 1),
                        j + 1 + (i + 1) * (pointsCount + 1), j + 1 + i * (pointsCount + 1)
                    }));
                }

                indices.AddRange(Triangulate(new[]
                {
                    pointsCount + i * (pointsCount + 1),
                    pointsCount + (i + 1) * (pointsCount + 1),
                    1u + (i + 1) * (pointsCount + 1),
                    1u + i * (pointsCount + 1)
                }));
            }

            if (capped)
            {
                indices.AddRange(Triangulate(0, pointsCount + 1));
                indices.AddRange(new[] {pointsCount, 0u, 1u});

                indices.AddRange(Triangulate((pointsCount + 1) * (sidesPropagation + 1),
                    (pointsCount + 1) * (sidesPropagation + 2)).Reverse());
                indices.AddRange(new[]
                    {
                        (pointsCount + 1) * (sidesPropagation + 2) - 1,
                        (pointsCount + 1) * (sidesPropagation + 1),
                        (pointsCount + 1) * (sidesPropagation + 1) + 1
                    }
                    .Reverse());
            }
            else
            {
                var tempIndices = new List<uint>(indices);
                indices.Reverse();
                tempIndices.AddRange(indices.Select(index => (uint) (index + vertices.Length)));
                indices = tempIndices;
                tempIndices.AddRange(indices);
                indices = tempIndices;

                var tempVertices = new List<Vertex>(vertices);
                tempVertices.AddRange(vertices);
                vertices = tempVertices.ToArray();
            }

            return new GeometryData(vertices, indices);
        }

        public static IList<T> Propagate<T>(T point1, T point2, int propagation = 1)
        {
            var propagatedPoints = new List<T>(2 * (propagation + 1));

            var deltaVector = (dynamic) point2 - point1;
            propagatedPoints.Add(point1);
            for (var j = 1; j <= propagation; ++j)
            {
                propagatedPoints.Add(point1 + deltaVector / (propagation + 1) * j);
            }

            return propagatedPoints;
        }

        public static IList<T> Propagate<T>(IList<T> points, int propagation = 1)
        {
            if (propagation < 1) return points;

            var propagatedPoints = new List<T>(points.Count * (propagation + 1));

            for (var i = 0; i < points.Count; ++i)
            {
                propagatedPoints.AddRange(Propagate(points[i], points[(i + 1) % points.Count], propagation));
            }

            return propagatedPoints;
        }

        public static IList<T> Propagate<T>(IList<T> points, float minDistance)
        {
            var propagatedPoints = new List<T>(points.Count);

            for (var i = 0; i < points.Count; ++i)
            {
                var distance = ((dynamic) points[(i + 1) % points.Count] - points[i]).Length;
                var propagation = (int) (distance / minDistance) - 1;
                propagatedPoints.AddRange(Propagate(points[i], points[(i + 1) % points.Count], propagation));
            }

            return propagatedPoints;
        }

        public static IEnumerable<uint> Triangulate(IEnumerable<uint> indices)
        {
            var enumerable = indices as uint[] ?? indices.ToArray();
            var trianglesIndices = new List<uint>((enumerable.Length - 2) * 3);

            var referenceIndex = enumerable[0];

            for (var i = 1; i < enumerable.Length - 1; ++i)
            {
                trianglesIndices.AddRange(new[] {referenceIndex, enumerable[i + 1], enumerable[i]});
            }

            return trianglesIndices;
        }

        public static IEnumerable<uint> Triangulate(uint startIndex, uint endIndex)
        {
            var trianglesIndices = new List<uint>((int) ((endIndex - startIndex - 2) * 3));

            for (var i = startIndex + 1; i < endIndex - 1; ++i)
            {
                trianglesIndices.AddRange(new[] {startIndex, i + 1, i});
            }

            return trianglesIndices;
        }

        public static Matrix4 CreateRotationMatrix(Vector3 rotationAngles)
        {
            var (x, y, z) = rotationAngles;
            return Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(z)) *
                   Matrix4.CreateRotationY(MathHelper.DegreesToRadians(x)) *
                   Matrix4.CreateRotationX(MathHelper.DegreesToRadians(y));
        }

        public static Matrix4 CreateTransform(Vector3 translation, Vector3 rotationAngles, Vector3 scaleFactors,
            Vector3 origin)
        {
            return Matrix4.CreateScale(scaleFactors) *
                   Matrix4.CreateTranslation(-origin) *
                   CreateRotationMatrix(rotationAngles) *
                   Matrix4.CreateTranslation(origin) *
                   Matrix4.CreateTranslation(translation);
        }

        public static GeometryData CalculateNormals(GeometryData data)
        {
            var vertices = data.Vertices;
            var indices = data.Indices;

            var normals = new Vector3[vertices.Count];
            var tangents = new Vector3[vertices.Count];

            for (var i = 0; i < indices.Count; i += 3)
            {
                var vertex1 = vertices[(int)indices[i]];
                var vertex2 = vertices[(int)indices[i + 1]];
                var vertex3 = vertices[(int)indices[i + 2]];

                var edge1 = vertex2.Position - vertex1.Position;
                var edge2 = vertex3.Position - vertex1.Position;

                var normalVector = Vector3.Cross(edge1, edge2);

                normals[(int)indices[i]] = normalVector;
                normals[(int)indices[i + 1]] = normalVector;
                normals[(int)indices[i + 2]] = normalVector;

                var deltaUv1 = vertex2.TextureCoords - vertex1.TextureCoords;
                var deltaUv2 = vertex3.TextureCoords - vertex1.TextureCoords;

                var f = 1.0f / (deltaUv1.X * deltaUv2.Y - deltaUv2.X * deltaUv1.Y);
                var tangent = f * deltaUv2.Y * edge1 - deltaUv1.Y * edge2;

                tangents[(int)indices[i]] = tangent;
                tangents[(int)indices[i + 1]] = tangent;
                tangents[(int)indices[i + 2]] = tangent;
            }

            for (var i = 0; i < vertices.Count; ++i)
                vertices[i] = new Vertex(vertices[i].Position, vertices[i].TextureCoords, normals[i].Normalized(),
                    tangents[i].Normalized());

            return data;
        }

        public static GeometryData CalculateNewellNormals(GeometryData data)
        {
            var vertices = data.Vertices;
            var indices = data.Indices;

            var normals = new Vector3[vertices.Count];
            var tangents = new Vector3[vertices.Count];

            for (var i = 0; i < indices.Count; i += 3)
            {
                var vertex1 = vertices[(int) indices[i]];
                var vertex2 = vertices[(int) indices[i + 1]];
                var vertex3 = vertices[(int) indices[i + 2]];

                var edge1 = vertex2.Position - vertex1.Position;
                var edge2 = vertex3.Position - vertex1.Position;
                var normalVector = Vector3.Cross(edge1, edge2);

                // var angle1 = Vector3.CalculateAngle((vertex2.Position - vertex1.Position),
                //     (vertex3.Position - vertex1.Position));
                // var angle2 = Vector3.CalculateAngle((vertex3.Position - vertex2.Position),
                //     (vertex1.Position - vertex2.Position));
                // var angle3 = Vector3.CalculateAngle((vertex1.Position - vertex3.Position),
                //     (vertex2.Position - vertex3.Position));

                normals[(int) indices[i]] += normalVector;
                normals[(int) indices[i + 1]] += normalVector;
                normals[(int) indices[i + 2]] += normalVector;

                var deltaUv1 = vertex2.TextureCoords - vertex1.TextureCoords;
                var deltaUv2 = vertex3.TextureCoords - vertex1.TextureCoords;

                var f = 1.0f / (deltaUv1.X * deltaUv2.Y - deltaUv2.X * deltaUv1.Y);
                var tangent = f * deltaUv2.Y * edge1 - deltaUv1.Y * edge2;

                tangents[(int) indices[i]] += tangent;
                tangents[(int) indices[i + 1]] += tangent;
                tangents[(int) indices[i + 2]] += tangent;
            }

            for (var i = 0; i < vertices.Count; ++i)
                vertices[i] = new Vertex(vertices[i].Position, vertices[i].TextureCoords, normals[i].Normalized(),
                    tangents[i].Normalized());

            return data;
        }

        public static IEnumerable<Vector2> Normalize(IEnumerable<Vector2> points)
        {
            var enumerable = points as Vector2[] ?? points.ToArray();
            var max = enumerable.First();
            max = enumerable.Select(point => new Vector2(System.Math.Abs(point.X), System.Math.Abs(point.Y)))
                .Aggregate(max, Vector2.ComponentMax);
            for (var i = 0; i < enumerable.Length; ++i)
            {
                enumerable[i] /= max;
            }

            return enumerable;
        }

        public static IEnumerable<Vector3> Normalize(IEnumerable<Vector3> points)
        {
            var enumerable = points as Vector3[] ?? points.ToArray();
            var max = enumerable.First();
            max = enumerable.Select(point =>
                    new Vector3(System.Math.Abs(point.X), System.Math.Abs(point.Y), System.Math.Abs(point.Z)))
                .Aggregate(max, Vector3.ComponentMax);
            for (var i = 0; i < enumerable.Length; ++i)
            {
                enumerable[i] /= max;
            }

            return enumerable;
        }

        public static Box3 CalculateBounds(IList<Vector3> points)
        {
            var minPoint = points.First();
            var maxPoint = points.First();

            foreach (var point in points)
            {
                minPoint = Vector3.ComponentMin(minPoint, point);
                maxPoint = Vector3.ComponentMax(maxPoint, point);
            }

            return new Box3(minPoint, maxPoint);
        }

        public static Box3 CalculateBounds(IEnumerable<Vertex> vertices)
        {
            return CalculateBounds(vertices.Select(v => v.Position).ToList());
        }

        public static IEnumerable<Vector3> Points(this Box3 box)
        {
            var topLeft = box.Max + new Vector3(-box.Size.X, 0, 0);
            var topRight = box.Max;
            var bottomLeft = box.Max + new Vector3(-box.Size.X, -box.Size.Y, 0);
            var bottomRight = box.Max + new Vector3(0, -box.Size.Y, 0);

            var bottomTopLeft = box.Min + new Vector3(box.Size.X, 0, 0);
            var bottomTopRight = box.Min;
            var bottomBottomLeft = box.Min + new Vector3(box.Size.X, box.Size.Y, 0);
            var bottomBottomRight = box.Min + new Vector3(0, box.Size.Y, 0);

            return new[]
            {
                topLeft, topRight, bottomLeft, bottomRight, bottomTopLeft, bottomBottomRight, bottomTopRight,
                bottomBottomLeft
            };
        }
    }
}