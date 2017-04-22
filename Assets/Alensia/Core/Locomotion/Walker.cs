using System;
using Alensia.Core.Physics;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Locomotion
{
    public class Walker : AnimatedLocomotion, IWalker
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

        protected override void Update(Vector3 velocity, Vector3 angularVelocity)
        {
            if (UseRootMotion)
            {
                Animator.applyRootMotion = GroundDetector.Grounded;
            }

            base.Update(velocity, angularVelocity);
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

        protected virtual void OnPacingChange(Pacing newPacing, Pacing oldPacing)
        {
            PacingChanged.Fire(newPacing, oldPacing);
        }
    }
}