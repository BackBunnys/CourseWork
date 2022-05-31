using System.Collections.Generic;

namespace CourseWork.Common.Geometry
{
    public class GeometryData
    {
        public IList<Vertex> Vertices { get; set; }
        public IList<uint> Indices { get; set; }

        public GeometryData(IList<Vertex> vertices, IList<uint> indices)
        {
            Vertices = vertices;
            Indices = indices;
        }
    }
}