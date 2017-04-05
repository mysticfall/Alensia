using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Actor
{
    public class Actor : IActor
    {
        public Animator Animator { get; private set; }

        public Transform Transform { get; private set; }

        public Actor(Animator animator, Transform transform)
        {
            Assert.IsNotNull(animator, "animator != null");
            Assert.IsNotNull(transform, "transform != null");

            Animator = animator;
            Transform = transform;
        }
    }
}