using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Character
{
    public class Character : ICharacter
    {
        public Animator Animator { get; }

        public Transform Transform { get; }

        public GameObject GameObject => Transform.gameObject;

        public Character(Animator animator, Transform transform)
        {
            Assert.IsNotNull(animator, "animator != null");
            Assert.IsNotNull(transform, "transform != null");

            Animator = animator;
            Transform = transform;
        }
    }
}