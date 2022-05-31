using System.Collections.Generic;
using System.Linq;
using CourseWork.Common.OpenGL;

namespace CourseWork.Common.Light
{
    public class LightContext
    {
        public List<DirectionalLight> DirectionalLights { get; } = new();
        public List<PointLight> PointLights { get; } = new();
        public List<SpotLight> SpotLights { get; } = new();

        public bool Enabled { get; private set; } = true;

        public bool Changed { get; private set; } = true;

        public LightContext()
        {
        }

        public LightContext(IReadOnlyCollection<DirectionalLight> directionalLights)
        {
            if (directionalLights != null)
                DirectionalLights.AddRange(directionalLights);
        }

        public LightContext(IReadOnlyCollection<PointLight> pointLights)
        {
            if (pointLights != null)
                PointLights.AddRange(pointLights);
        }

        public LightContext(IReadOnlyCollection<SpotLight> spotLights)
        {
            if (spotLights != null)
                SpotLights.AddRange(spotLights);
        }

        public LightContext(IReadOnlyCollection<DirectionalLight> directionalLights,
            IReadOnlyCollection<PointLight> pointLights, IReadOnlyCollection<SpotLight> spotLights)
        {
            if (directionalLights != null)
                DirectionalLights.AddRange(directionalLights);
            if (pointLights != null)
                PointLights.AddRange(pointLights);
            if (spotLights != null)
                SpotLights.AddRange(spotLights);
        }

        public static LightContext Of(DirectionalLight light)
        {
            var context = new LightContext();
            context.DirectionalLights.Add(light);
            return context;
        }

        public static LightContext Of(PointLight light)
        {
            var context = new LightContext();
            context.PointLights.Add(light);
            return context;
        }

        public static LightContext Of(SpotLight light)
        {
            var context = new LightContext();
            context.SpotLights.Add(light);
            return context;
        }

        public void Combine(LightContext anotherContext)
        {
            DirectionalLights.AddRange(anotherContext.DirectionalLights);
            PointLights.AddRange(anotherContext.PointLights);
            SpotLights.AddRange(anotherContext.SpotLights);
            Changed = true;
        }

        public void Combine(IEnumerable<LightContext> anotherContexts)
        {
            foreach (var context in anotherContexts)
            {
                Combine(context);
            }
        }

        public void Apply(ShaderProgram shader)
        {
            Apply(DirectionalLights, shader, "directionalLightCount");
            Apply(PointLights, shader, "pointLightCount");
            Apply(SpotLights, shader, "spotLightCount");
            Changed = false;
        }

        public void Enable() => ToggleAllLights(true);

        public void Disable() => ToggleAllLights(false);

        private static void Apply<T>(IEnumerable<T> lights, ShaderProgram shader, string countParam) where T : ILight
        {
            var enabledLights = lights.Where(light => light.Enabled).ToList();
            shader.Use();
            shader.SetUniform(countParam, enabledLights.Count);

            for (var i = 0; i < enabledLights.Count; ++i)
            {
                enabledLights[i].Apply(i, shader);
            }
        }

        private void ToggleAllLights(bool enabled)
        {
            Enabled = enabled;
            ToggleLights(DirectionalLights, enabled);
            ToggleLights(PointLights, enabled);
            ToggleLights(SpotLights, enabled);
        }

        private static void ToggleLights<T>(IEnumerable<T> lights, bool enabled) where T : class, ILight
        {
            foreach (var light in lights)
            {
                light.Enabled = enabled;
            }
        }
    }
}