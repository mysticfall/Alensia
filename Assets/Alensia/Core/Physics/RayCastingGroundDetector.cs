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
            var origin = Origin;
            var distance = MaximumDistance;

            Collider ground = null;

            while (ground == null && distance > 0)
            {
                RaycastHit hit;

                var ray = new Ray(origin, Vector3.down);

                if (UnityEngine.Physics.Raycast(ray, out hit, MaximumDistance, Settings.GroundLayer))
                {
                    if (IsGround(hit.collider))
                    {
                        ground = hit.collider;
                    }
                    else
                    {
                        origin += new Vector3(0, hit.distance, 0);
                    }
                }
                else
                {
                    distance = 0;
                }
            }

            OnDetectGround(ground);
        }

        public virtual void FixedTick()
        {
            DetectGround();
        }
    }
}