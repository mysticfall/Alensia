using UnityEngine;
using Zenject;

namespace Alensia.Core.Locomotion
{
    public class TransformDrivenLocomotion : AnimatedLocomotion
    {
        public bool ApplyMovement;

        public bool ApplyRotation;

        public TransformDrivenLocomotion(
            Animator animator,
            Transform transform) : this(new Settings(), animator, transform)
        {
        }

        [Inject]
        public TransformDrivenLocomotion(
            Settings settings,
            Animator animator,
            Transform transform) : base(settings, animator, transform)
        {
            //TODO Temporary code to test combined locomotion mode.
            ApplyMovement = !animator.applyRootMotion;
            ApplyRotation = true;
        }

        public override Vector3 Move(Vector3 velocity)
        {
            if (ApplyMovement)
            {
                var global = Transform.InverseTransformVector(velocity);

                Transform.position += global * Time.deltaTime;
            }

            base.Move(velocity);

            return velocity;
        }

        public override Vector3 Rotate(Vector3 velocity)
        {
            if (ApplyRotation)
            {
                var target = velocity * Time.deltaTime;

                Transform.localRotation *= Quaternion.Euler(target.x, target.y, target.z);
            }

            base.Rotate(velocity);

            return velocity;
        }
    }
}