using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class LineCastingGroundDetector : RayCastingGroundDetector<Collider>
    {
        public LineCastingGroundDetector(Collider target) :
            this(new GroundDetectionSettings(), target)
        {
        }

        [Inject]
        public LineCastingGroundDetector(
            GroundDetectionSettings settings, Collider target) : base(settings, target)
        {
        }

        protected override Vector3 CalculateOrigin(Collider target)
        {
            var bounds = Target.bounds;

            return bounds.center - new Vector3(0, bounds.extents.y - 0.001f);
        }

        protected override RaycastHit[] CastRay(Ray ray, Collider target)
        {
            return UnityEngine.Physics.RaycastAll(
                ray,
                Settings.Tolerance,
                Settings.GroundLayer);
        }
    }
}