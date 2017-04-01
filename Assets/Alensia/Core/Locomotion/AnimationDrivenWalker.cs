using UnityEngine;

namespace Alensia.Core.Locomotion
{
    public class AnimationBasedWalker : BaseWalker
    {
        public override Vector3 Velocity
        {
            get { return Transform.InverseTransformDirection(Animator.velocity); }
        }

        public AnimationBasedWalker(
            WalkSpeedSettings maximumSpeed,
            WalkAnimationVariables walkAnimationVariables,
            Transform transform,
            Animator animator) : base(maximumSpeed, walkAnimationVariables, transform, animator)
        {
            Animator.applyRootMotion = true;
            Animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        }

        public override void Move(Vector3 direction, float desiredSpeed)
        {
            var target = direction * desiredSpeed;

            var z = Animator.GetFloat(WalkAnimationVariables.ForwardVelocity);
            var x = Animator.GetFloat(WalkAnimationVariables.SidewayVelocity);

            var current = new Vector3(x, target.y, z);
            var velocity = Vector3.LerpUnclamped(current, target, Time.deltaTime * 3.0f);

            Animator.SetBool(WalkAnimationVariables.Moving, velocity.magnitude > 0);
            Animator.SetFloat(WalkAnimationVariables.ForwardVelocity, velocity.z);
            Animator.SetFloat(WalkAnimationVariables.SidewayVelocity, velocity.x);
        }

        public override void Rotate(Vector3 rotation, float desiredSpeed)
        {
            var target = Transform.localEulerAngles + rotation;
            var aspect = Quaternion.Euler(target.x, target.y, target.z);

            Transform.localRotation = Quaternion.RotateTowards(
                Transform.localRotation, aspect, desiredSpeed * Time.deltaTime);
        }
    }
}