using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Physics
{
    public abstract class RayCastingGroundDetector<T> : GroundDetector, IFixedTickable
        where T : Collider
    {
        public override GroundDetectionSettings Settings
        {
            get { return _settings; }
        }

        public override Collider Target
        {
            get { return _target; }
        }

        private readonly GroundDetectionSettings _settings;

        private readonly T _target;

        protected RayCastingGroundDetector(
            T target,
            GroundHitEvent groundHit,
            GroundLeaveEvent groundLeft) :
            this(new GroundDetectionSettings(), target, groundHit, groundLeft)
        {
        }

        [Inject]
        protected RayCastingGroundDetector(
            GroundDetectionSettings settings,
            T target,
            GroundHitEvent groundHit,
            GroundLeaveEvent groundLeft) : base(groundHit, groundLeft)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(target, "target != null");

            _settings = settings;
            _target = target;
        }

        protected virtual void DetectGround()
        {
            var hits = CastRay(CreateRay(), _target);
            var grounds = hits.Select(h => h.collider).Where(h => h != Target && IsGround(h));

            OnDetectGround(grounds);
        }

        protected abstract Vector3 CalculateOrigin(T target);

        protected virtual Ray CreateRay()
        {
            return new Ray(CalculateOrigin(_target), Vector3.down);
        }

        protected abstract RaycastHit[] CastRay(Ray ray, T target);

        public virtual void FixedTick()
        {
            DetectGround();
        }
    }
}