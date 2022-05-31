using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace CourseWork.Common.Geometry
{
    public record IntersectionResult(IIntersectable Target, IIntersectable With, Vector3 Velocity);
    public class IntersectionObserver
    {
        public ConcurrentDictionary<IIntersectable, Box3> Observables { get; } = new();

        public event Action<IntersectionResult> Intersected;

        public void Register(IIntersectable intersectable)
        {
            if (intersectable == null) throw new ArgumentException("Intersectable can't be null");

            void Action() => CheckIntersections(intersectable);

            intersectable.Changed += Action;

            Observables.AddOrUpdate(intersectable, intersectable.Bounds, (_, box3) => box3);
        }

        private void CheckIntersections(IIntersectable intersectable)
        {
            foreach (var (observable, bounds) in Observables)
            {
                if (observable == intersectable) continue;
                
                var result = observable.Intersects(intersectable);
                var intersectableResult = intersectable.Intersects(observable);

                var previousBounds = Observables.GetOrAdd(intersectable, intersectable.Bounds);
                Observables.TryUpdate(intersectable, intersectable.Bounds, previousBounds);

                if (result == null || intersectableResult == null) continue;

                //Console.WriteLine(intersectable.Bounds);

                var velocity = intersectable.Bounds.Center - previousBounds.Center;
                OnIntersected(new IntersectionResult(intersectableResult, result, velocity));
            }
        }

        protected virtual void OnIntersected(IntersectionResult result)
        {
            Intersected?.Invoke(result);
        }
    }
}