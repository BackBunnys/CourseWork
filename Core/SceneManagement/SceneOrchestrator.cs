using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Core.SceneManagement
{
    internal class SceneOrchestrator
    {
        public List<Scene> Scenes { get; }
        public Scene CurrentScene => Scenes.ElementAtOrDefault(_currentSceneIndex);
        private int _currentSceneIndex = -1;

        public SceneOrchestrator(ICollection<Scene> scenes)
        {
            if (scenes.Count < 1)
                throw new ArgumentException("There are must be at least 1 scene!");

            Scenes = scenes.ToList();
            NextScene();
        }

        private void NextScene()
        {
            var previousScene = CurrentScene;
            
            _currentSceneIndex++;
            
            if (_currentSceneIndex >= Scenes.Count) return;
            
            CurrentScene.Load();
            CurrentScene.OnFinish += NextScene;

            previousScene?.Unload();
        }
    }
}
