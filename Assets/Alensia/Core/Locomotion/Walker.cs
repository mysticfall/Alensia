using System;
using Alensia.Core.Physics;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Locomotion
{
    public class Walker : AnimatedLocomotion, IWalker, IInitializable, IDisposable
    {
        public WalkSpeedSettings MaximumSpeed { get; set; }

        public IGroundDetector GroundDetector { get; private set; }

        private Pacing _pacing = Pacing.Walking();

        public Pacing Pacing
        {
            get { return _pacing; }
            set
            {
                Assert.IsNotNull(value, "Pacing != null");

                var oldPacing = _pacing;

                _pacing = value;

                if (oldPacing != _pacing)
                {
                    OnPacingChange(_pacing, oldPacing);
                }
            }
        }

        public PacingChangeEvent PacingChanged { get; private set; }

        private bool _rootMotionApplied;

        public Walker(
            IGroundDetector groundDetector,
            Animator animator,
            Transform transform,
            PacingChangeEvent pacingChanged) :
            this(new WalkSpeedSettings(), groundDetector, animator, transform, pacingChanged)
        {
        }

        [Inject]
        public Walker(
            WalkSpeedSettings maximumSpeed,
            IGroundDetector groundDetector,
            Animator animator,
            Transform transform,
            PacingChangeEvent pacingChanged) : base(animator, transform)
        {
            Assert.IsNotNull(maximumSpeed, "maximumSpeed != null");
            Assert.IsNotNull(groundDetector, "groundDetector != null");
            Assert.IsNotNull(pacingChanged, "pacingChanged != null");

            MaximumSpeed = maximumSpeed;
            PacingChanged = pacingChanged;
            GroundDetector = groundDetector;
        }

        public void Initialize()
        {
            GroundDetector.GroundHit.Listen(OnHitGround);
            GroundDetector.GroundLeft.Listen(OnLeaveGround);

            _rootMotionApplied = Animator.applyRootMotion;
        }

        public void Dispose()
        {
            GroundDetector.GroundHit.Unlisten(OnHitGround);
            GroundDetector.GroundLeft.Unlisten(OnLeaveGround);
        }

        public void Walk(Vector2 direction, float heading)
        {
            throw new NotImplementedException();
        }

        public void Jump(Vector2 direction)
        {
            throw new NotImplementedException();
        }

        private Vector3 _lastVelocity;

        protected override Vector3 CalculateVelocity(Vector3 direction, float? distance = null)
        {
            if (!GroundDetector.Grounded) return Vector3.zero;

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
            if (!GroundDetector.Grounded) return Vector3.zero;

            var maximumSpeed = MaximumSpeed.Angular;

            var speed = degrees.HasValue
                ? Mathf.Clamp(degrees.Value / Time.deltaTime, -maximumSpeed, maximumSpeed)
                : maximumSpeed;

            return axis * speed;
        }

        protected virtual void OnHitGround(Collider ground)
        {
            if (!UseRootMotion) return;

            Animator.applyRootMotion = _rootMotionApplied;
        }

        protected virtual void OnLeaveGround(Collider ground)
        {
            if (!UseRootMotion) return;

            _rootMotionApplied = Animator.applyRootMotion;

            Animator.applyRootMotion = false;
        }

        protected override void UpdateVelocity(Vector3 velocity)
        {
            if (!GroundDetector.Grounded) return;

            var target = Transform.position +
                         Transform.rotation * velocity * Time.deltaTime;

            Transform.position = target;
        }

        protected override void UpdateRotation(Vector3 angularVelocity)
        {
            if (!GroundDetector.Grounded) return;

            var angle = (angularVelocity * Time.deltaTime).magnitude;
            var rotation = Quaternion.AngleAxis(angle, angularVelocity.normalized);

            Transform.localRotation *= rotation;
        }

        protected virtual void OnPacingChange(Pacing newPacing, Pacing oldPacing)
        {
            PacingChanged.Fire(newPacing, oldPacing);
        }
    }
}