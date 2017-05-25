using System;
using Alensia.Core.Actor;
using Alensia.Core.Common;
using Alensia.Core.Geom;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Camera
{
    public class HeadMountedCamera : RotatableCamera, IFirstPersonCamera, ILateTickable
    {
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
            set { _settings.LookAhead = value; }
        }

        public Vector3 CameraOffset
        {
            get { return _settings.CameraOffset; }
            set { _settings.CameraOffset = value; }
        }

        public override bool Valid => base.Valid && Head != null;

        public override RotationalConstraints RotationalConstraints => _settings.Rotation;

        public ITransformable Target { get; private set; }

        public Transform Head { get; private set; }

        public override Vector3 Pivot
        {
            get
            {
                var humanoid = Target as IHumanoid;
                var offset = Head.TransformDirection(CameraOffset) *
                             CameraOffset.magnitude;

                return (humanoid?.Viewpoint ?? Head.position) + offset;
            }
        }

        public override Vector3 AxisUp => _settings.HeadAxisUp.Of(Head);

        public override Vector3 AxisForward => _settings.HeadAxisFoward.Of(Head);

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

            _initialRotation = Head.localRotation;
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();

            if (Head == null) return;

            Head.localRotation = _initialRotation;
        }

        protected virtual void UpdatePosition(float heading, float elevation)
        {
            Head.localRotation = Quaternion.identity;

            Head.Rotate(AxisUp, heading, Space.World);
            Head.Rotate(AxisRight, -elevation, Space.World);

            Transform.position = Pivot;
            Transform.rotation = Quaternion.LookRotation(AxisForward, AxisUp);

            Transform.LookAt(FocalPoint);
        }

        public virtual void LateTick()
        {
            if (Active) UpdatePosition(Heading, Elevation);
        }

        [Serializable]
        public class Settings : IEditorSettings
        {
            public RotationalConstraints Rotation = new RotationalConstraints
            {
                Down = 65,
                Side = 85,
                Up = 60
            };

            [Range(0.1f, 10f)]
            public float LookAhead = 10f;

            public Vector3 CameraOffset = new Vector3(0, 0, 0.03f);

            public Axis HeadAxisUp = Axis.Y;

            public Axis HeadAxisFoward = Axis.Z;
        }
    }
}