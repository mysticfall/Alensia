using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Physics
{
    public class CollisionDetector : MonoBehaviour, ICollisionDetector
    {
        [Inject]
        public Collider Target { get; private set; }

        [Inject]
        public CollisionEnterEvent CollisionEntered { get; private set; }

        [Inject]
        public CollisionExitEvent CollisionExited { get; private set; }

        protected virtual void OnValidate()
        {
            Assert.IsNotNull(Target, "Target != null");

            Assert.IsNotNull(CollisionEntered, "CollisionEntered != null");
            Assert.IsNotNull(CollisionExited, "CollisionExited != null");
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            CollisionEntered.Fire(collision);
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            CollisionExited.Fire(collision);
        }
    }
}