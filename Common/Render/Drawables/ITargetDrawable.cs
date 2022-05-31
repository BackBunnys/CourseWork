using CourseWork.Common.Render.Targets;

namespace CourseWork.Common.Render.Drawables
{
    public interface ITargetDrawable
    {
        void Draw(RenderTarget target, RenderStates states);
    }
}
