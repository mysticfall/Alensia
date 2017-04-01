using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Locomotion
{
    public abstract class BaseWalker : IWalker
    {
        public Transform Transform { get; private set; }

        public Animator Animator { get; private set; }

        public WalkSpeedSettings MaximumSpeed { get; set; }

        public WalkAnimationVariables WalkAnimationVariables { get; set; }

        public abstract Vector3 Velocity { get;  }

        private Pacing _pacing = Pacing.Walking();

        public Pacing Pacing
        {
            get { return _pacing; }
            set { _pacing = value; }
        }

        public event EventHandler<PacingChangeEventArgs> PacingChanged;

        protected BaseWalker(
            WalkSpeedSettings maximumSpeed,
            WalkAnimationVariables walkAnimationVariables,
            Transform transform,
            Animator animator)
        {
            Assert.IsNotNull(maximumSpeed, "maximumSpeed != null");
            Assert.IsNotNull(walkAnimationVariables, "walkAnimationVariables != null");
            Assert.IsNotNull(transform, "transform != null");
            Assert.IsNotNull(animator, "animator != null");

            MaximumSpeed = maximumSpeed;
            WalkAnimationVariables = walkAnimationVariables;
            Transform = transform;
            Animator = animator;
        }

        public abstract void Move(Vector3 direction, float desiredSpeed);

        public abstract void Rotate(Vector3 rotation, float desiredSpeed);

        public virtual void Walk(Vector2 direction)
        {
            var vector = new Vector3(direction.x, 0, direction.y);

            Vector3 desiredVelocity;

            var magnitude = direction.magnitude;

            if (magnitude > 0)
            {
                var forwardRatio = direction.y / magnitude;
                var forwardSpeed = direction.y > 0 ? MaximumSpeed.Forward : MaximumSpeed.Backward;

                var sideRatio = direction.x / magnitude;

                desiredVelocity =
                    new Vector2(MaximumSpeed.Sideway * sideRatio, forwardSpeed * forwardRatio);
            }
            else
            {
                desiredVelocity = Vector3.zero;
            }

            Move(vector.normalized, desiredVelocity.magnitude);
        }

        public virtual void Turn(float degrees)
        {
            if (!Mathf.Approximately(degrees, 0))
            {
                Rotate(new Vector3(0, degrees, 0), MaximumSpeed.Angular);
            }
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