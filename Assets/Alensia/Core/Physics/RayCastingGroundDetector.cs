using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public abstract class RayCastingGroundDetector<T> : GroundDetector, IFixedTickable
        where T : Collider
    {
        public override Collider Target => _target;

        [Inject] private T _target;

        protected virtual void DetectGround()
        {
            var hits = CastRay(CreateRay(), _target);
            var grounds = hits.Select(h => h.collider).Where(h => h != Target && IsGround(h));

            OnDetectGround(grounds);
        }

        protected abstract Vector3 CalculateOrigin(T target);

        protected virtual Ray CreateRay() => new Ray(CalculateOrigin(_target), Vector3.down);

        protected abstract IEnumerable<RaycastHit> CastRay(Ray ray, T target);

        public void FixedTick() => DetectGround();
    }
}