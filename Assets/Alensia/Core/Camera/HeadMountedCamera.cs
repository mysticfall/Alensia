using Alensia.Core.Character;
using Alensia.Core.Geom;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Camera
{
    public class HeadMountedCamera : RotatableCamera, IFirstPersonCamera, ILateTickable
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

        private readonly IReactiveProperty<float> _heading;

        private readonly IReactiveProperty<float> _elevation;

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
            if (Valid && Active) UpdatePosition(Heading, Elevation);
        }
    }
}