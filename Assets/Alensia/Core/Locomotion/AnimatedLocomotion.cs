using System;
using Alensia.Core.Animation;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Locomotion
{
    public class AnimatedLocomotion : ILocomotion, IAnimatable
    {
        public MovementVariables MovementVariables { get; private set; }

        public RotationVariables RotationVariables { get; private set; }

        public Animator Animator { get; private set; }

        public Transform Transform { get; private set; }

        public AnimatedLocomotion(
            Animator animator, Transform transform) : this(new Settings(), animator, transform)
        {
        }

        [Inject]
        public AnimatedLocomotion(Settings settings, Animator animator, Transform transform)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(animator, "animator != null");
            Assert.IsNotNull(transform, "transform != null");

            MovementVariables = settings.MovementVariables;
            RotationVariables = settings.RotationVariables;

            Assert.IsNotNull(MovementVariables, "settings.MovementVariables != null");
            Assert.IsNotNull(RotationVariables, "settings.RotationVariables != null");

            Animator = animator;
            Transform = transform;
        }

        public virtual Vector3 Move(Vector3 velocity)
        {
            var x = Animator.GetFloat(MovementVariables.SpeedRight);
            var y = Animator.GetFloat(MovementVariables.SpeedUp);
            var z = Animator.GetFloat(MovementVariables.SpeedForward);

            var current = new Vector3(x, y, z);
            var effective = Vector3.LerpUnclamped(current, velocity, Time.deltaTime * 3.0f);

            Animator.SetBool(MovementVariables.Moving, effective.magnitude > 0);

            Animator.SetFloat(MovementVariables.SpeedRight, effective.x);
            Animator.SetFloat(MovementVariables.SpeedUp, effective.y);
            Animator.SetFloat(MovementVariables.SpeedForward, effective.z);

            return effective;
        }

        public virtual Vector3 Rotate(Vector3 velocity)
        {
            var x = Animator.GetFloat(RotationVariables.SpeedPitch);
            var y = Animator.GetFloat(RotationVariables.SpeedYaw);
            var z = Animator.GetFloat(RotationVariables.SpeedRoll);

            var current = new Vector3(x, y, z);
            var effective = Vector3.LerpUnclamped(current, velocity, Time.deltaTime * 3.0f);

            Animator.SetBool(RotationVariables.Turning, effective.magnitude > 0);

            Animator.SetFloat(RotationVariables.SpeedPitch, effective.x);
            Animator.SetFloat(RotationVariables.SpeedYaw, effective.y);
            Animator.SetFloat(RotationVariables.SpeedRoll, effective.z);

            return effective;
        }

        [Serializable]
        public class Settings : IEditorSettings
        {
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