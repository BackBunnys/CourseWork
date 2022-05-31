using CourseWork.Common;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using CourseWork.Common.Render.Targets;
using OpenTK.Mathematics;

namespace CourseWork.Objects
{
    public class FigurePair<T> : Transformable, ITargetDrawable where T : Figure
    {
        public T Left { get; }
        public T Right { get; }

        protected FigurePair(T left, T right, Vector3 offset)
        {
            Left = left;
            Left.Position = -offset;

            Right = right;
            Right.Position = offset;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            var localStates = states with {Transform = TransformMatrix * states.Transform};
            Left.Draw(target, localStates);
            Right.Draw(target, localStates);
        }
    }
}