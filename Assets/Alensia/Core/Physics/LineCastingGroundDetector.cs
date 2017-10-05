using UnityEngine;

namespace Alensia.Core.Physics
{
    public class LineCastingGroundDetector : RayCastingGroundDetector<Collider>
    {
        protected override Vector3 CalculateOrigin(Collider target)
        {
            var bounds = Target.bounds;

            return bounds.center - new Vector3(0, bounds.extents.y - 0.001f);
        }

        protected override RaycastHit[] CastRay(Ray ray, Collider target)
        {
            return UnityEngine.Physics.RaycastAll(ray, Tolerance, GroundLayer);
        }
    }
}