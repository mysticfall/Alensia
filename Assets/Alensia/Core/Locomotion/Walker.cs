using System;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Locomotion
{
    public class Walker : IWalker
    {
        public WalkSpeedSettings MaximumSpeed { get; set; }

        public ILocomotion Locomotion { get; private set; }

        private Pacing _pacing = Pacing.Walking();

        public Pacing Pacing
        {
            get { return _pacing; }
            set { _pacing = value; }
        }

        public event EventHandler<PacingChangeEventArgs> PacingChanged;

        public Walker(ILocomotion locomotion) : this(new WalkSpeedSettings(), locomotion)
        {
        }

        [Inject]
        public Walker(
            WalkSpeedSettings maximumSpeed,
            ILocomotion locomotion)
        {
            Assert.IsNotNull(maximumSpeed, "maximumSpeed != null");
            Assert.IsNotNull(locomotion, "locomotion != null");

            MaximumSpeed = maximumSpeed;
            Locomotion = locomotion;
        }

        public virtual void Walk(Vector2 direction)
        {
            Vector3 desiredVelocity;

            var magnitude = direction.magnitude;

            if (magnitude > 0)
            {
                var forwardRatio = direction.y / magnitude;
                var forwardSpeed = direction.y > 0 ? MaximumSpeed.Forward : MaximumSpeed.Backward;

                var sideRatio = direction.x / magnitude;

                desiredVelocity = new Vector3
                {
                    x = MaximumSpeed.Sideway * sideRatio,
                    y = 0,
                    z = forwardSpeed * forwardRatio
                };
            }
            else
            {
                desiredVelocity = Vector3.zero;
            }

            Locomotion.Move(desiredVelocity);
        }

        public virtual void WalkTo(Vector3 position)
        {
            throw new NotImplementedException();
        }

        public virtual void Turn(float direction)
        {
            var speed = Math.Sign(direction) * MaximumSpeed.Angular;

            Locomotion.Rotate(Vector3.up * speed);
        }

        public virtual void TurnTo(float heading)
        {
            var current = Locomotion.Transform.localEulerAngles.y;
            var delta = GeometryUtils.NormalizeAspectAngle(heading - current);

            var speed = Mathf.Clamp(
                delta / Time.deltaTime,
                -MaximumSpeed.Angular,
                MaximumSpeed.Angular);

            Locomotion.Rotate(Vector3.up * speed);
        }

        public virtual void Jump(Vector2 direction)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnPacingChange(PacingChangeEventArgs args)
        {
            if (PacingChanged != null)
            {
                PacingChanged(this, new PacingChangeEventArgs(Pacing));
            }
        }
    }
}