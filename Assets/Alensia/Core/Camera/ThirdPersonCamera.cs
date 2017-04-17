using System;
using Alensia.Core.Actor;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Camera
{
    public class ThirdPersonCamera : OrbitingCamera, IThirdPersonCamera
    {
        public override RotationalConstraints RotationalConstraints
        {
            get { return _settings.Rotation; }
        }

        public override DistanceSettings DistanceSettings
        {
            get { return _settings.Distance; }
        }

        public WallAvoidanceSettings WallAvoidanceSettings
        {
            get { return _settings.WallAvoidance; }
        }

        public override bool Valid
        {
            get { return base.Valid && Target != null; }
        }

        public ITransformable Target { get; private set; }

        public override Vector3 Pivot
        {
            get
            {
                var humanoid = Target as IHumanoid;

                return humanoid != null ? humanoid.Viewpoint : Target.Transform.position;
            }
        }

        public override Vector3 AxisForward
        {
            get { return Target.Transform.forward; }
        }

        public override Vector3 AxisUp
        {
            get { return Target.Transform.up; }
        }

        private readonly Settings _settings;

        public ThirdPersonCamera(UnityEngine.Camera camera) : this(new Settings(), camera)
        {
        }

        [Inject]
        public ThirdPersonCamera(
            Settings settings,
            UnityEngine.Camera camera) : base(camera)
        {
            Assert.IsNotNull(settings, "settings != null");

            _settings = settings;
        }

        public void Initialize(ITransformable target)
        {
            Assert.IsNotNull(target, "target != null");

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