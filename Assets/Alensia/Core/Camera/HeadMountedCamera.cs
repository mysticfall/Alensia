using System;
using Alensia.Core.Character;
using Alensia.Core.Geom;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public class HeadMountedCamera : RotatableCamera, IFirstPersonCamera
    {
        public override float Heading
        {
            get { return _heading.Value; }
            set
            {
                _heading.Value = Mathf.Clamp(
                    GeometryUtils.NormalizeAspectAngle(value),
                    -RotationalConstraints.Side,
                    RotationalConstraints.Side);
            }
        }

        public override float Elevation
        {
            get { return _elevation.Value; }
            set
            {
                _elevation.Value = Mathf.Clamp(
                    GeometryUtils.NormalizeAspectAngle(value),
                    -RotationalConstraints.Down,
                    RotationalConstraints.Up);
            }
        }

        public float LookAhead
        {
            get { return _lookAhead; }
            set { _lookAhead = value; }
        }

        public Vector3 CameraOffset
        {
            get { return _cameraOffset; }
            set { _cameraOffset = value; }
        }

        public override bool Valid => base.Valid && Head != null;

        public override RotationalConstraints RotationalConstraints => _rotation;

        public ICharacter Target { get; private set; }

        public Transform Head { get; private set; }

        public override Vector3 Pivot
        {
            get
            {
                var offset = Head.TransformDirection(CameraOffset) * CameraOffset.magnitude;

                return (Target?.Vision?.Pivot ?? Head.position) + offset;
            }
        }

        public override Vector3 AxisUp => _headAxisUp.Of(Head);

        public override Vector3 AxisForward => _headAxisFoward.Of(Head);

        public FocusSettings FocusSettings => _focus;

        public Transform Focus => Active && Valid ? _focused.Value : null;

        public IObservable<Transform> OnFocusChange => _focused.Where(_ => Active && Valid);

        protected Vector3 FocalPoint
        {
            get
            {
                var rotation = Target.Transform.rotation * Quaternion.Euler(-Elevation, Heading, 0);

                return Head.position + rotation * Vector3.forward * LookAhead;
            }
        }

        [SerializeField] private RotationalConstraints _rotation;

        [SerializeField] [Range(0.1f, 10f)] private float _lookAhead = 10f;

        [SerializeField] private Vector3 _cameraOffset = new Vector3(0, 0, 0.03f);

        [SerializeField] private Axis _headAxisUp = Axis.Y;

        [SerializeField] private Axis _headAxisFoward = Axis.Z;

        [SerializeField] private FocusSettings _focus;

        private readonly IReactiveProperty<float> _heading;

        private readonly IReactiveProperty<float> _elevation;

        private readonly IReactiveProperty<Transform> _focused;

        private Quaternion _initialRotation;

        public HeadMountedCamera()
        {
            _heading = new ReactiveProperty<float>();
            _elevation = new ReactiveProperty<float>();

            _rotation = new RotationalConstraints
            {
                Down = 65,
                Side = 85,
                Up = 60
            };

            _focus = new FocusSettings();
            _focused = new ReactiveProperty<Transform>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            OnDeactivate
                .Where(_ => Head != null)
                .Subscribe(_ => Head.localRotation = _initialRotation)
                .AddTo(this);

            Observable
                .Zip(_heading, _elevation)
                .Where(_ => Valid && Active)
                .Subscribe(args => UpdatePosition(args[0], args[1]))
                .AddTo(this);
        }

        public void Track(ICharacter target)
        {
            Assert.IsNotNull(target, "target != null");

            Target = target;
            Head = Target?.Head ?? Target?.Transform;

            _initialRotation = Head.localRotation;
        }

        public virtual void UpdatePosition()
        {
            UpdatePosition(Heading, Elevation);
            UpdateFocus();
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

        protected virtual void UpdateFocus()
        {
            if (!FocusSettings.TrackFocus)
            {
                _focused.Value = null;

                return;
            }

            var ray = new Ray(Transform.position, Transform.forward);
            var distance = FocusSettings.MaximumDistance;
            var layer = FocusSettings.Layer;

            RaycastHit hit;

            _focused.Value = UnityEngine.Physics.Raycast(ray, out hit, distance, layer) ? hit.transform : null;
        }
    }
}