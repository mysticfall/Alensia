using Alensia.Core.Character;
using UnityEngine;

namespace Alensia.Core.Camera
{
    public class ThirdPersonCamera : OrbitingCamera, IThirdPersonCamera
    {
        public override RotationalConstraints RotationalConstraints => _rotation;

        public override DistanceSettings DistanceSettings => _distance;

        public WallAvoidanceSettings WallAvoidanceSettings => _wallAvoidance;

        public override bool Valid => base.Valid && Target != null;

        public ICharacter Target { get; private set; }

        public override Vector3 Pivot => Target.Vision?.Pivot ?? Target.Transform.position;

        public override Vector3 AxisForward => Target.Transform.forward;

        public override Vector3 AxisUp => Target.Transform.up;

        [SerializeField] private RotationalConstraints _rotation;

        [SerializeField] private DistanceSettings _distance;

        [SerializeField] private WallAvoidanceSettings _wallAvoidance;

        public ThirdPersonCamera()
        {
            _rotation = new RotationalConstraints
            {
                Down = 80,
                Side = 180,
                Up = 80
            };

            _distance = new DistanceSettings();
            _wallAvoidance = new WallAvoidanceSettings();
        }

        public void Track(ICharacter target)
        {
            Target = target;
            Distance = DistanceSettings.Default;
        }

        protected override void UpdatePosition(float heading, float elevation, float distance)
        {
            var preferredDistance = distance;

            if (WallAvoidanceSettings.AvoidWalls)
            {
                var direction = (Transform.position - Pivot).normalized;
                var origin = Pivot + direction * DistanceSettings.Minimum;

                var ray = new Ray(origin, direction);

                RaycastHit hit;

                if (UnityEngine.Physics.Raycast(ray, out hit, preferredDistance))
                {
                    preferredDistance =
                        Vector3.Distance(Pivot, hit.point) -
                        WallAvoidanceSettings.MinimumDistance;
                }
            }

            base.UpdatePosition(heading, elevation, preferredDistance);
        }
    }
}