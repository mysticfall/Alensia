using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Actor
{
    public class Actor : IActor
    {
        public Transform Transform { get; private set; }

        public Animator Animator { get; private set; }

        public Actor(Transform transform, Animator animator)
        {
            Assert.IsNotNull(transform, "transform != null");
            Assert.IsNotNull(animator, "animator != null");

            Transform = transform;
            Animator = animator;
        }
    }
}