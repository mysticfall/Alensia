using System;
using Alensia.Core.Animation;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Locomotion
{
    public abstract class AnimatedLocomotion : Locomotion, IAnimatable
    {
        public bool UseRootMotionForMovement => _settings.UseRootMotionForMovement;

        public bool UseRootMotionForRotation => _settings.UseRootMotionForRotation;

        public bool UseRootMotion => UseRootMotionForMovement || UseRootMotionForRotation;

        public MovementVariables MovementVariables => _settings.MovementVariables;

        public RotationVariables RotationVariables => _settings.RotationVariables;

        public Animator Animator { get; }

        private readonly Settings _settings;

        protected AnimatedLocomotion(
            Animator animator,
            Transform transform) : this(new Settings(), animator, transform)
        {
        }

        [Inject]
        protected AnimatedLocomotion(
            Settings settings,
            Animator animator,
            Transform transform) : base(transform)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(animator, "animator != null");

            _settings = settings;

            Animator = animator;
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            Animator.applyRootMotion = UseRootMotion;
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();

            UpdateVelocityVariables(Vector3.zero);
            UpdateRotationVariables(Vector3.zero);

            Animator.applyRootMotion = false;
        }

        protected override void Update(Vector3 velocity, Vector3 angularVelocity)
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

        [Serializable]
        public class Settings : IEditorSettings
        {
            public bool UseRootMotionForMovement = true;

            public bool UseRootMotionForRotation = false;

            public MovementVariables MovementVariables = new MovementVariables();

            public RotationVariables RotationVariables = new RotationVariables();
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