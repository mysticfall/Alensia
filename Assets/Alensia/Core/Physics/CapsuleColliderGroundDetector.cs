using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class CapsuleColliderGroundDetector : RayCastingGroundDetector<CapsuleCollider>
    {
        public CapsuleColliderGroundDetector(CapsuleCollider target) :
            this(new GroundDetectionSettings(), target)
        {
        }

        [Inject]
        public CapsuleColliderGroundDetector(
            GroundDetectionSettings settings, CapsuleCollider target) : base(settings, target)
        {
        }

        protected override Vector3 CalculateOrigin(CapsuleCollider target)
        {
            var bounds = Target.bounds;
            var offset = new Vector3(0, bounds.extents.y - target.radius - 0.001f);

            return bounds.center - offset;
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