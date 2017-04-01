using UnityEngine;

namespace Alensia.Core.Locomotion
{
    public class TransformBasedWalker : BaseWalker
    {
        private Vector3 _velocity;

        public override Vector3 Velocity
        {
            get { return _velocity; }
        }

        public TransformBasedWalker(
            WalkSpeedSettings maximumSpeed,
            WalkAnimationVariables walkAnimationVariables,
            Transform transform,
            Animator animator) : base(maximumSpeed, walkAnimationVariables, transform, animator)
        {
            Animator.applyRootMotion = true;
        }

        public override void Move(Vector3 direction, float desiredSpeed)
        {
            var localVelocity = direction.normalized * desiredSpeed;
            var velocity = Transform.TransformDirection(localVelocity);

            _velocity = Vector3.LerpUnclamped(Velocity, velocity, Time.deltaTime * 1.5f);

            Transform.position += Velocity * Time.deltaTime;

            Animator.SetBool(WalkAnimationVariables.Moving, velocity.magnitude > 0);
            Animator.SetFloat(WalkAnimationVariables.ForwardVelocity, _velocity.z);
            Animator.SetFloat(WalkAnimationVariables.SidewayVelocity, _velocity.x);
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