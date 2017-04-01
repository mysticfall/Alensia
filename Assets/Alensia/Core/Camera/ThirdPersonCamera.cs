using System;
using Alensia.Core.Actor;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public class ThirdPersonCamera : BaseOrbitingCamera, IThirdPersonCamera
    {
        public override RotationalConstraints RotationalConstraints
        {
            get { return _rotationalConstraints; }
        }

        public override DistanceSettings DistanceSettings
        {
            get { return _distanceSettings; }
        }

        public WallAvoidanceSettings WallAvoidanceSettings
        {
            get { return _wallAvoidanceSettings; }
        }

        public override bool Valid
        {
            get { return base.Valid && _target != null; }
        }

        protected override Vector3 Anchor
        {
            get { return _anchor.position; }
        }

        protected override Vector3 AxisForward
        {
            get { return _target.forward; }
        }

        protected override Vector3 AxisUp
        {
            get { return _target.up; }
        }

        private Transform _target;

        private Transform _anchor;

        private readonly RotationalConstraints _rotationalConstraints;

        private readonly DistanceSettings _distanceSettings;

        private readonly WallAvoidanceSettings _wallAvoidanceSettings;

        public ThirdPersonCamera(
            Settings settings,
            UnityEngine.Camera camera) : base(camera)
        {
            Assert.IsNotNull(settings, "settings != null");

            _rotationalConstraints = settings.Rotation;
            _distanceSettings = settings.Distance;
            _wallAvoidanceSettings = settings.WallAvoidance;
        }

        public void Initialize(ITransformable target)
        {
            Assert.IsNotNull(target, "target != null");

            var character = target as IHumanoid;

            _target = target.Transform;
            _anchor = character != null ? character.GetBodyPart(HumanBodyBones.Head) : _target;

            Distance = DistanceSettings.Default;
        }

        protected override void UpdatePosition(
            float heading, float elevation, float distance)
        {
            var preferredDistance = distance;

            if (WallAvoidanceSettings.AvoidWalls)
            {
                var direction = (Transform.position - Anchor).normalized;
                var ray = new Ray(Anchor, direction);

                RaycastHit hit;

                if (UnityEngine.Physics.Raycast(ray, out hit, preferredDistance))
                {
                    preferredDistance =
                        Vector3.Distance(Anchor, hit.point) - WallAvoidanceSettings.MinimumDistance;
                }
            }

            base.UpdatePosition(heading, elevation, preferredDistance);
        }

        [Serializable]
        public class Settings : IEditorSettings
        {
            public RotationalConstraints Rotation;

            public DistanceSettings Distance;

            public WallAvoidanceSettings WallAvoidance;
        }
    }
}