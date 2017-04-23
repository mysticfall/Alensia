using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Physics
{
    public class RayCastingGroundDetector : GroundDetector, IFixedTickable
    {
        public override GroundDetectionSettings Settings
        {
            get { return _settings; }
        }

        public override Collider Target
        {
            get { return _target; }
        }

        public virtual Vector3 Origin
        {
            get
            {
                var bounds = Target.bounds;

                return bounds.center - new Vector3(0, bounds.extents.y - 0.001f);
            }
        }

        protected virtual float MaximumDistance
        {
            get { return Settings.Tolerance; }
        }

        private readonly GroundDetectionSettings _settings;

        private readonly Collider _target;

        public RayCastingGroundDetector(
            Collider target,
            GroundHitEvent groundHit,
            GroundLeaveEvent groundLeft) :
            this(new GroundDetectionSettings(), target, groundHit, groundLeft)
        {
        }

        [Inject]
        public RayCastingGroundDetector(
            GroundDetectionSettings settings,
            Collider target,
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
            var ray = new Ray(Origin, Vector3.down);
            var hits = UnityEngine.Physics.RaycastAll(ray, MaximumDistance, Settings.GroundLayer);

            var grounds = hits.Select(h => h.collider).Where(IsGround);

            OnDetectGround(grounds);
        }

        public virtual void FixedTick()
        {
            DetectGround();
        }
    }
}