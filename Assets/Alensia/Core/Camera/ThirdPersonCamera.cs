using System;
using Alensia.Core.Character;
using Alensia.Core.Common;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Camera
{
    public class ThirdPersonCamera : OrbitingCamera, IThirdPersonCamera
    {
        public override RotationalConstraints RotationalConstraints => _settings.Rotation;

        public override DistanceSettings DistanceSettings => _settings.Distance;

        public WallAvoidanceSettings WallAvoidanceSettings => _settings.WallAvoidance;

        public override bool Valid => base.Valid && Target != null;

        public ICharacter Target { get; private set; }

        public override Vector3 Pivot => Target.Vision?.Pivot ?? Target.Transform.position;

        public override Vector3 AxisForward => Target.Transform.forward;

        public override Vector3 AxisUp => Target.Transform.up;

        private readonly Settings _settings;

        public ThirdPersonCamera(UnityEngine.Camera camera) : this(null, camera)
        {
        }

        [Inject]
        public ThirdPersonCamera(
            [InjectOptional] Settings settings,
            UnityEngine.Camera camera) : base(camera)
        {
            _settings = settings ?? new Settings();
        }

        public void Track(ICharacter target)
        {
            Target = target;
            Distance = DistanceSettings.Default;
        }

        protected override void UpdatePosition(
            float heading, float elevation, float distance)
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

        [Serializable]
        public class Settings : IEditorSettings
        {
            public RotationalConstraints Rotation = new RotationalConstraints
            {
                Down = 80,
                Side = 180,
                Up = 80
            };

            public DistanceSettings Distance = new DistanceSettings();

            public WallAvoidanceSettings WallAvoidance = new WallAvoidanceSettings();
        }
    }
}