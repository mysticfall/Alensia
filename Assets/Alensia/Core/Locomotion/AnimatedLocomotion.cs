using System;
using Alensia.Core.Animation;
using Alensia.Core.Common;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Locomotion
{
    public abstract class AnimatedLocomotion : Locomotion, IAnimatable
    {
        [Inject]
        public Animator Animator { get; }

        public bool UseRootMotionForMovement => _useRootMotionForMovement;

        public bool UseRootMotionForRotation => _useRootMotionForRotation;

        public bool UseRootMotion => UseRootMotionForMovement || UseRootMotionForRotation;

        public MovementVariables MovementVariables => _movementVariables;

        public RotationVariables RotationVariables => _rotationVariables;

        [SerializeField] private bool _useRootMotionForMovement = true;

        [SerializeField] private bool _useRootMotionForRotation;

        [SerializeField] private MovementVariables _movementVariables;

        [SerializeField] private RotationVariables _rotationVariables;

        protected AnimatedLocomotion()
        {
            _movementVariables = new MovementVariables();
            _rotationVariables = new RotationVariables();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            Animator.applyRootMotion = UseRootMotion;
        }

        protected override void OnDeactivated()
        {
            UpdateVelocityVariables(Vector3.zero);
            UpdateRotationVariables(Vector3.zero);

            Animator.applyRootMotion = false;

            base.OnDeactivated();
        }

        protected override void UpdateVelocity(Vector3 velocity, Vector3 angularVelocity)
        {
            UpdateVelocityVariables(velocity);

            if (!UseRootMotionForMovement || !Animator.applyRootMotion)
            {
                UpdateVelocity(velocity);
            }

            UpdateRotationVariables(angularVelocity);

            if (!UseRootMotionForRotation || !Animator.applyRootMotion)
            {
                UpdateRotation(angularVelocity);
            }
        }

        protected virtual void UpdateVelocityVariables(Vector3 velocity)
        {
            Animator.SetBool(MovementVariables.Moving, velocity.magnitude > 0);

            Animator.SetFloat(MovementVariables.SpeedRight, velocity.x);
            Animator.SetFloat(MovementVariables.SpeedUp, velocity.y);
            Animator.SetFloat(MovementVariables.SpeedForward, velocity.z);
        }

        protected virtual void UpdateRotationVariables(Vector3 angularVelocity)
        {
            Animator.SetBool(RotationVariables.Turning, angularVelocity.magnitude > 0);

            Animator.SetFloat(RotationVariables.SpeedPitch, angularVelocity.x);
            Animator.SetFloat(RotationVariables.SpeedYaw, angularVelocity.y);
            Animator.SetFloat(RotationVariables.SpeedRoll, angularVelocity.z);
        }
    }

    [Serializable]
    public class MovementVariables : IEditorSettings
    {
        public string Moving = "Moving";

        public string SpeedForward = "SpeedForward";

        public string SpeedRight = "SpeedRight";

        public string SpeedUp = "SpeedUp";
    }

    [Serializable]
    public class RotationVariables : IEditorSettings
    {
        public string Turning = "Turning";

        public string SpeedPitch = "SpeedPitch";

        public string SpeedRoll = "SpeedRoll";

        public string SpeedYaw = "SpeedYaw";
    }
}