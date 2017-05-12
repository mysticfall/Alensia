using System;
using Alensia.Core.Actor;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Camera
{
    public class HeadMountedCamera : RotatableCamera, IFirstPersonCamera, ILateTickable
    {
        public const string MountPointName = "CameraMount";

        public override float Heading
        {
            get { return _heading; }

            set
            {
                var heading = Mathf.Clamp(
                    GeometryUtils.NormalizeAspectAngle(value),
                    -RotationalConstraints.Side,
                    RotationalConstraints.Side);

                _heading = heading;

                UpdatePosition(heading, Elevation);
            }
        }

        public override float Elevation
        {
            get { return _elevation; }

            set
            {
                var elevation = Mathf.Clamp(
                    GeometryUtils.NormalizeAspectAngle(value),
                    -RotationalConstraints.Down,
                    RotationalConstraints.Up);

                _elevation = elevation;

                UpdatePosition(Heading, elevation);
            }
        }

        public float LookAhead
        {
            get { return _settings.LookAhead; }
        }

        public override bool Valid
        {
            get { return base.Valid && Head != null; }
        }

        public override RotationalConstraints RotationalConstraints
        {
            get { return _settings.Rotation; }
        }

        public ITransformable Target { get; private set; }

        public Transform Head { get; private set; }

        public override Vector3 Pivot
        {
            get
            {
                var humanoid = Target as IHumanoid;

                return !_hasMountPoint && humanoid != null ? humanoid.Viewpoint : _pivotObject.position;
            }
        }

        public override Vector3 AxisForward
        {
            get { return _pivotObject.forward; }
        }

        public override Vector3 AxisUp
        {
            get { return _pivotObject.up; }
        }

        protected Vector3 FocalPoint
        {
            get
            {
                var rotation = Target.Transform.rotation * Quaternion.Euler(-Elevation, Heading, 0);

                return Head.position + rotation * Vector3.forward * LookAhead;
            }
        }

        private float _heading;

        private float _elevation;

        private Quaternion _initialRotation;

        private Transform _pivotObject;

        private bool _hasMountPoint;

        private readonly Settings _settings;

        public HeadMountedCamera(
            UnityEngine.Camera camera) : this(new Settings(), camera)
        {
        }

        [Inject]
        public HeadMountedCamera(
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

            var character = target as IHumanoid;

            if (character == null)
            {
                Head = Target.Transform;
            }
            else
            {
                Head = character.Head ?? Target.Transform;
            }

            _pivotObject = FindMountPoint(Head) ?? Head;
            _hasMountPoint = Head != _pivotObject;

            _initialRotation = Head.localRotation;
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();

            if (Head == null) return;

            Head.localRotation = _initialRotation;
        }

        protected virtual Transform FindMountPoint(Transform parent)
        {
            Assert.IsNotNull(parent, "parent != null");

            return parent.FindChild(MountPointName);
        }

        protected virtual void UpdatePosition(float heading, float elevation)
        {
            if (Head == _pivotObject)
            {
                Head.localRotation = Quaternion.Euler(new Vector3(-elevation, heading, 0));
            }
            else
            {
                Head.localRotation = _initialRotation *
                                     _pivotObject.localRotation *
                                     Quaternion.Euler(new Vector3(-elevation, heading, 0)) *
                                     Quaternion.Inverse(_pivotObject.localRotation);
            }

            Transform.position = Pivot;
            Transform.rotation = Quaternion.LookRotation(AxisForward, AxisUp);

            if (Mathf.Abs(elevation) > 89)
            {
                Transform.LookAt(FocalPoint, Transform.up);
            }
            else
            {
                Transform.LookAt(FocalPoint);
            }
        }

        public virtual void LateTick()
        {
            if (Active) UpdatePosition(Heading, Elevation);
        }

        [Serializable]
        public class Settings : IEditorSettings
        {
            [Range(0.1f, 10f)]
            public float LookAhead = 10f;

            public RotationalConstraints Rotation = new RotationalConstraints
            {
                Down = 65,
                Side = 85,
                Up = 60
            };
        }
    }
}