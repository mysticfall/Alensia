using System.Collections.Generic;
using UnityEngine;

namespace Alensia.Core.Physics
{
    public class CapsuleColliderGroundDetector : RayCastingGroundDetector<CapsuleCollider>
    {
        protected override Vector3 CalculateOrigin(CapsuleCollider target)
        {
            var bounds = Target.bounds;
            var offset = new Vector3(0, bounds.extents.y - target.radius - 0.001f);

            return bounds.center - offset;
        }

        protected override IEnumerable<RaycastHit> CastRay(Ray ray, CapsuleCollider target)
        {
            return UnityEngine.Physics.SphereCastAll(
                ray,
                target.radius,
                Tolerance,
                GroundLayer);
        }
    }
}