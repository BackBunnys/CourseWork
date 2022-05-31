using System.Collections.Generic;
using CourseWork.Core.AssetManagement;
using OpenTK.Mathematics;

namespace CourseWork.Objects.Environment
{
    public class Tree : Model
    {
        public static readonly RotationBody Base;
        public readonly RotationBody Head;

        static Tree()
        {
            var generatrix = new[]
            {
                new Vector2(0.5f, -0.5f), 
                new Vector2(0.4f, 0.5f)
            };
            Base = new RotationBody(generatrix, 36, Vector3.UnitX)
            {
                Color = new Color4(105, 65, 44, 255),
                Size = new Vector3(0.3f, 3f, 0.3f),
                Position = new Vector3(0, 0, 0),
                Texture = AssetManager.GetTexture("bark"),
                TextureTiling = new Vector3(2, 6, 2),
            };
        }

        public Tree(uint sides)
        {
            Head = new RotationBody(CreateLevyPoints(8), sides, Vector3.UnitX)
            {
                Color = Color4.DarkGreen,
                Position = new Vector3(0, 1.5f, 0),
                Rotation = new Vector3(0, 180, 0)
            };
            Drawables.Add(Base);
            Drawables.Add(Head);
            InitBounds();
        }

        private static IEnumerable<Vector2> CreateLevyPoints(int numberOfIterations)
        {
            IList<Vector2> points = new List<Vector2>();
            LevyIteration(new Vector2(-1, 1), new Vector2(1, 1), numberOfIterations, points);

            return points;
        }

        private static void LevyIteration(Vector2 leftPoint, Vector2 rightPoint, int iterationNumber,
            ICollection<Vector2> points)
        {
            if (iterationNumber == 0)
            {
                points.Add(leftPoint);
                points.Add(rightPoint);
            }
            else
            {
                var point = new Vector2(
                    (leftPoint.X + rightPoint.X) / 2 + (rightPoint.Y - leftPoint.Y) / 2,
                    (leftPoint.Y + rightPoint.Y) / 2 - (rightPoint.X - leftPoint.X) / 2);

                LevyIteration(leftPoint, point, iterationNumber - 1, points);
                LevyIteration(point, rightPoint, iterationNumber - 1, points);
            }
        }
    }
}