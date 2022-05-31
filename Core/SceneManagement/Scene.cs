using System;
using System.Collections.Generic;
using CourseWork.Common.Render;
using CourseWork.Common.Render.Drawables;
using CourseWork.Common.Render.Targets;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CourseWork.Core.SceneManagement
{
    public abstract class Scene
    {
        private bool _finished;
        protected readonly KeyboardState KeyboardState;
        protected readonly RenderTarget RenderTarget;
        protected RenderStates RenderStates;
        protected readonly List<ITargetDrawable> Drawables = new();

        protected bool Finished
        {
            get => _finished;
            set
            {
                _finished = value;
                if (_finished)
                {
                    OnFinish?.Invoke();
                }
            }
        }

        public event Action OnFinish;

        protected Scene(RenderTarget renderTarget, RenderStates renderStates, KeyboardState keyboardState)
        {
            RenderTarget = renderTarget;
            RenderStates = renderStates;
            KeyboardState = keyboardState;
        }

        public virtual void Load()
        {
        }

        public virtual void Unload()
        {

        }

        public virtual void HandleKeyDown(KeyboardKeyEventArgs e)
        {
        }

        public virtual void HandleKeyUp(KeyboardKeyEventArgs e)
        {
        }

        public virtual void HandleInput(MouseButtonEventArgs e)
        {
        }

        public virtual void HandleInput(MouseWheelEventArgs e)
        {
        }

        public virtual void HandleInput(MouseMoveEventArgs e)
        {
        }

        public abstract void Update(double dt);

        public virtual void Draw()
        {
            Drawables.ForEach(drawable => drawable.Draw(RenderTarget, RenderStates));
        }
    }
}