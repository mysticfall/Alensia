using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class CapsuleColliderGroundDetector : RayCastingGroundDetector<CapsuleCollider>
    {
        public CapsuleColliderGroundDetector(
            CapsuleCollider target,
            GroundHitEvent groundHit,
            GroundLeaveEvent groundLeft) :
            this(new GroundDetectionSettings(), target, groundHit, groundLeft)
        {
        }

        [Inject]
        public CapsuleColliderGroundDetector(
            GroundDetectionSettings settings,
            CapsuleCollider target,
            GroundHitEvent groundHit,
            GroundLeaveEvent groundLeft) : base(settings, target, groundHit, groundLeft)
        {
        }

        protected override Vector3 CalculateOrigin(CapsuleCollider target)
        {
            var bounds = Target.bounds;

            return bounds.center - new Vector3(0, bounds.extents.y - target.radius);
        }

        protected override RaycastHit[] CastRay(Ray ray, CapsuleCollider target)
        {
            return UnityEngine.Physics.SphereCastAll(
                ray,
                target.radius,
                Settings.Tolerance,
                Settings.GroundLayer);
        }
    }
}