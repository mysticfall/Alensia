using System;
using Alensia.Core.Character;
using UniRx;
using UnityEngine;
using UEPhysics = UnityEngine.Physics;

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

        public FocusSettings FocusSettings => _focus;

        public Transform Focus => Active && Valid ? _focused.Value : null;

        public IObservable<Transform> OnFocusChange => _focused.Where(_ => Active && Valid);

        [SerializeField] private RotationalConstraints _rotation;

        [SerializeField] private DistanceSettings _distance;

        [SerializeField] private WallAvoidanceSettings _wallAvoidance;

        [SerializeField] private FocusSettings _focus;

        private readonly IReactiveProperty<Transform> _focused;

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

            _focus = new FocusSettings
            {
                TrackFocus = false
            };

            _focused = new ReactiveProperty<Transform>();
        }

        public void Track(ICharacter target)
        {
            Target = target;
            Distance = DistanceSettings.Default;
        }

        public override void UpdatePosition()
        {
            base.UpdatePosition();

            if (FocusSettings.TrackFocus)
            {
                UpdateFocus();
            }
        }

        protected override void UpdatePosition(float heading, float elevation, float distance)
        {
            base.UpdatePosition(heading, elevation, GetUnblockedDistance(distance));
        }

        protected virtual float GetUnblockedDistance(float distance)
        {
            if (!WallAvoidanceSettings.AvoidWalls)
            {
                return distance;
            }

            var direction = (Transform.position - Pivot).normalized;
            var origin = Pivot + direction * DistanceSettings.Minimum;

            var ray = new Ray(origin, direction);

            RaycastHit hit;

            if (UEPhysics.Raycast(ray, out hit, distance))
            {
                distance =
                    Vector3.Distance(Pivot, hit.point) -
                    WallAvoidanceSettings.MinimumDistance;
            }

            return distance;
        }

        protected virtual void UpdateFocus()
        {
            var direction = (Pivot - Transform.position).normalized;
            var ray = new Ray(Pivot, direction);

            RaycastHit hit;

            var hasHit = UEPhysics.Raycast(
                ray, out hit, FocusSettings.MaximumDistance, FocusSettings.Layer);

            _focused.Value = hasHit ? hit.transform : null;
        }
    }
}