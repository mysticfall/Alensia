using Alensia.Core.Physics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Locomotion
{
    public class PhysicsDrivenLocomotion : AnimatedLocomotion, IPhysicalObject
    {
        public Rigidbody Body { get; private set; }

        public PhysicsDrivenLocomotion(
            Settings settings,
            Rigidbody body,
            Animator animator,
            Transform transform) : base(settings, animator, transform)
        {
            Assert.IsNotNull(body, "body != null");

            Body = body;
        }

        public override Vector3 Move(Vector3 velocity)
        {
            var current = Transform.InverseTransformVector(Body.velocity);
            var desired = Transform.TransformVector(velocity - current);

            Body.AddForce(desired, ForceMode.VelocityChange);

            var effective = Transform.InverseTransformVector(Body.velocity);

            base.Move(effective);

            return effective;
        }

        public override Vector3 Rotate(Vector3 velocity)
        {
            var current = Transform.InverseTransformVector(Body.angularVelocity);
            var desired = Transform.TransformVector(velocity - current);

            Body.AddTorque(desired, ForceMode.VelocityChange);

            var effective = Transform.InverseTransformVector(Body.angularVelocity);

            base.Rotate(effective);

            return effective;
        }
    }
}