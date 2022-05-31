using System;

namespace CourseWork.Common.OpenGL
{
    public abstract class Bindable
    {
        public abstract void Bind();
        public abstract void Unbind();

        protected void RunWithBinding(Action action)
        {
            Bind();
            action.Invoke();
            Unbind();
        }
    }
}
