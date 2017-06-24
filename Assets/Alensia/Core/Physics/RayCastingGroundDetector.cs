using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Physics
{
    public abstract class RayCastingGroundDetector<T> : GroundDetector, IFixedTickable
        where T : Collider
    {
        public override GroundDetectionSettings Settings { get; }

        public override Collider Target => _target;

        private readonly T _target;

        protected RayCastingGroundDetector(T target) : this(new GroundDetectionSettings(), target)
        {
        }

        [Inject]
        protected RayCastingGroundDetector(GroundDetectionSettings settings, T target)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(target, "target != null");

            Settings = settings;
            _target = target;
        }

        protected virtual void DetectGround()
        {
            var hits = CastRay(CreateRay(), _target);
            var grounds = hits.Select(h => h.collider).Where(h => h != Target && IsGround(h));

            OnDetectGround(grounds);
        }

        protected abstract Vector3 CalculateOrigin(T target);

        protected virtual Ray CreateRay() => new Ray(CalculateOrigin(_target), Vector3.down);

        protected abstract RaycastHit[] CastRay(Ray ray, T target);

        public void FixedTick() => DetectGround();
    }
}