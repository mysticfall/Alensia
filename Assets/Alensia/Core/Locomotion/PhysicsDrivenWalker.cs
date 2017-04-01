using Alensia.Core.Physics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Locomotion
{
    public class PhysicsBasedLocomotion : ILocomotion, IPhysicalObject
    {
        public Transform Transform { get; private set; }

        public Rigidbody Body { get; private set; }

        public PhysicsBasedLocomotion(Transform transform, Rigidbody body)
        {
            Transform = transform;
            Body = body;

            Assert.IsNotNull(Transform);
            Assert.IsNotNull(Body);
        }

        public Vector3 Velocity { get; private set; }

        public void Move(Vector3 direction, float desiredSpeed)
        {
            Body.AddForce(direction * desiredSpeed, ForceMode.Force);
        }

        public void Rotate(Vector3 rotation, float desiredSpeed)
        {
            Body.AddTorque(Transform.up * desiredSpeed);
        }
    }
}