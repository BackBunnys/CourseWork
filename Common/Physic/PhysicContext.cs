using System.Collections.Concurrent;
using CourseWork.Common.Geometry;
using OpenTK.Mathematics;

namespace CourseWork.Common.Physic
{
    public class PhysicContext : IUpdateable
    {
        public class Effect
        {
            public Vector3 Velocity { get; set; }
            public float Lifetime { get; set; }
            public float Duration { get; set; }

            public Effect(Vector3 velocity, float lifetime)
            {
                Velocity = velocity;
                Lifetime = lifetime;
            }
        }

        public ConcurrentDictionary<ITransformable, Effect> Effects { get; } = new();

        public void ModelCollision(IIntersectable target, IIntersectable with, Vector3 velocity)
        {
            var effect = new Effect(-velocity * 2f, 1);
            Effects.AddOrUpdate(target, effect,
                (_, _) => effect);
        }

        public void Update(double dt)
        {
            foreach (var (target, effect) in Effects)
            {
                var factor = (effect.Lifetime - effect.Duration) / effect.Lifetime;
                target.Move(effect.Velocity * factor * factor * factor);
                effect.Duration += (float) dt;
                if (effect.Duration >= effect.Lifetime)
                    Effects.TryRemove(target, out _);
            }
        }
    }
}