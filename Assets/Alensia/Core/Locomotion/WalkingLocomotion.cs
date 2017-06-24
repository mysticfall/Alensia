using System;
using Alensia.Core.Common;
using Alensia.Core.Physics;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Locomotion
{
    public class WalkingLocomotion : AnimatedLocomotion, IWalkingLocomotion
    {
        public WalkSpeedSettings MaximumSpeed => _settings.MaximumSpeed;

        public LocomotionVariables JumpingAndFallingVariables => _settings.JumpingAndFallingVariables;

        public IGroundDetector GroundDetector { get; }

        public IReactiveProperty<Pacing> Pacing { get;  }

        private Vector3 _lastVelocity;

        private readonly Settings _settings;

        public WalkingLocomotion(
            IGroundDetector groundDetector,
            Animator animator,
            Transform transform) :
            this(new Settings(), groundDetector, animator, transform)
        {
        }

        [Inject]
        public WalkingLocomotion(
            Settings settings,
            IGroundDetector groundDetector,
            Animator animator,
            Transform transform) : base(settings, animator, transform)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(groundDetector, "groundDetector != null");

            _settings = settings;

            Pacing = new ReactiveProperty<Pacing>(Core.Locomotion.Pacing.Walking());
            GroundDetector = groundDetector;

            groundDetector.OnGroundHit.Subscribe(_ => WhenHitGround()).AddTo(this);
            groundDetector.OnGroundLeave.Subscribe(_ => WhenLeaveGround()).AddTo(this);
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

                speed *= Pacing.Value.SpeedModifier;
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

        protected virtual void WhenHitGround()
        {
            if (!Active.Value) Activate();

            Animator.SetBool(JumpingAndFallingVariables.Falling, false);
        }

        protected virtual void WhenLeaveGround()
        {
            if (GroundDetector.Grounded.Value) return;

            if (Active.Value) Deactivate();

            Animator.SetBool(JumpingAndFallingVariables.Falling, true);
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
        public new class Settings : AnimatedLocomotion.Settings
        {
            public WalkSpeedSettings MaximumSpeed = new WalkSpeedSettings();

            public LocomotionVariables JumpingAndFallingVariables = new LocomotionVariables();
        }

        [Serializable]
        public class LocomotionVariables : IEditorSettings
        {
            public string Jumping = "Jumping";

            public string Falling = "Falling";
        }
    }
}