using System;
using System.Collections.Generic;
using CourseWork.Common;
using CourseWork.Common.Geometry;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class Sphere : Figure
    {
        public Sphere(float radius, int sectors = 16, int stacks = 16) : base(CreateMesh(radius, sectors, stacks))
        {
        }

        private static Mesh CreateMesh(float radius, int sectorCount, int stackCount)
        {
            var vertices = new List<Vertex>();
            var indices = new List<uint>();

            var lengthInv = 1.0f / radius; // normal

            var sectorStep = (float) (2 * Math.PI / sectorCount);
            var stackStep = (float) (Math.PI / stackCount);

            for (var i = 0; i <= stackCount; ++i)
            {
                var stackAngle = (float) (Math.PI / 2 - i * stackStep);
                var xy = (float) (radius * Math.Cos(stackAngle)); // vertex position
                var z = (float) (radius * Math.Sin(stackAngle)); // vertex position

                // add (sectorCount+1) vertices per stack
                // the first and last vertices have same position and normal, but different tex coords
                for (var j = 0; j <= sectorCount; ++j)
                {
                    var sectorAngle = j * sectorStep;

                    // vertex position
                    var x = (float) (xy * Math.Cos(sectorAngle)); // vertex position
                    var y = (float) (xy * Math.Sin(sectorAngle)); // vertex position

                    // normalized vertex normal
                    var nx = x * lengthInv; // normal
                    var ny = y * lengthInv; // normal
                    var nz = z * lengthInv; // normal

                    // vertex tex coord between [0, 1]
                    var s = (float) j / sectorCount; // texCoord
                    var t = (float) i / stackCount; // texCoord

                    vertices.Add(new Vertex(new Vector3(x, y, z), new Vector2(s, t), new Vector3(nx, ny, nz)));
                }
            }

            // indices
            //  k1--k1+1
            //  |  / |
            //  | /  |
            //  k2--k2+1
            for (var i = 0u; i < stackCount; ++i)
            {
                var k1 = (uint) (i * (sectorCount + 1));
                var k2 = (uint) (k1 + sectorCount + 1);

                for (var j = 0u; j < sectorCount; ++j, ++k1, ++k2)
                {
                    // 2 triangles per sector excluding 1st and last stacks
                    if (i != 0)
                    {
                        indices.AddRange(new[] {k1, k2, k1 + 1});
                    }

                    if (i != (stackCount - 1))
                    {
                        indices.AddRange(new[] {k1 + 1, k2, k2 + 1});
                    }
                }
            }

            // generate interleaved vertex array as well
            //   buildInterleavedVertices();
            return new Mesh(vertices, indices);
        }
    }
}