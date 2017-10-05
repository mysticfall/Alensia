using System;
using Alensia.Core.Common;
using Alensia.Core.Physics;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Locomotion
{
    public class LeggedLocomotion : AnimatedLocomotion, ILeggedLocomotion
    {
        public WalkSpeedSettings MaximumSpeed => _maximumSpeed;

        public LocomotionVariables JumpingAndFallingVariables => _jumpingAndFallingVariables;

        [Inject]
        public IGroundDetector GroundDetector { get; }

        public Pacing Pacing
        {
            get { return _pacing.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _pacing.Value = value;
            }
        }

        public IObservable<Pacing> OnPacingChange => _pacing;

        private Vector3 _lastVelocity;

        [SerializeField] private LocomotionVariables _jumpingAndFallingVariables;

        [SerializeField] private WalkSpeedSettings _maximumSpeed;

        [SerializeField, HideInInspector] private PacingReactiveProperty _pacing;

        public LeggedLocomotion()
        {
            _pacing = new PacingReactiveProperty(Pacing.Walking());

            _maximumSpeed = new WalkSpeedSettings();
            _jumpingAndFallingVariables = new LocomotionVariables();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            GroundDetector.OnGroundedStateChange
                .Subscribe(OnGroundedStateChange, Debug.LogError)
                .AddTo(this);
        }

        public void Walk(Vector2 direction, float heading)
        {
            Move(direction);
            RotateTowards(Vector3.up, heading);
        }

        public void Jump(Vector2 direction)
        {
            throw new NotImplementedException();
        }

        protected override Vector3 CalculateVelocity(Vector3 direction, float? distance = null)
        {
            var magnitude = direction.magnitude;

            float speed = 0;

            if (magnitude > 0)
            {
                var forwardRatio = direction.z / magnitude;
                var sideRatio = direction.x / magnitude;

                var forwardSpeed = direction.z > 0 ? MaximumSpeed.Forward : MaximumSpeed.Backward;
                var sideSpeed = MaximumSpeed.Sideway;

                speed = Mathf.Sqrt(
                    Mathf.Pow(sideSpeed * sideRatio, 2) +
                    Mathf.Pow(forwardSpeed * forwardRatio, 2));

                if (distance.HasValue)
                {
                    var maximumSpeed = distance.Value / Time.deltaTime;
                    speed = Mathf.Min(speed, maximumSpeed);
                }

                speed *= Pacing.SpeedModifier;
            }

            // Do proper interpolation / smoothing.
            var velocity = Vector3.Lerp(_lastVelocity, direction * speed, Time.deltaTime * 5f);

            _lastVelocity = velocity;

            return velocity;
        }

        protected override Vector3 CalculateAngularVelocity(Vector3 axis, float? degrees = null)
        {
            var maximumSpeed = MaximumSpeed.Angular;

            var speed = degrees.HasValue
                ? Mathf.Clamp(degrees.Value / Time.deltaTime, -maximumSpeed, maximumSpeed)
                : maximumSpeed;

            return axis * speed;
        }

        protected virtual void OnGroundedStateChange(bool grounded)
        {
            Active = grounded;

            Animator.SetBool(JumpingAndFallingVariables.Falling, !grounded);
        }

        protected override void UpdateVelocity(Vector3 velocity)
        {
            var target = Transform.position +
                         Transform.rotation * velocity * Time.deltaTime;

            Transform.position = target;
        }

        protected override void UpdateRotation(Vector3 angularVelocity)
        {
            var angle = (angularVelocity * Time.deltaTime).magnitude;
            var rotation = Quaternion.AngleAxis(angle, angularVelocity.normalized);

            Transform.localRotation *= rotation;
        }

        [Serializable]
        public class LocomotionVariables : IEditorSettings
        {
            public string Jumping = "Jumping";

            public string Falling = "Falling";
        }
    }
}