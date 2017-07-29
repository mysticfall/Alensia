using Alensia.Core.Character.Generic;
using Alensia.Core.Common;
using Alensia.Core.Locomotion;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Character
{
    public abstract class Character<TLocomotion> : BaseObject, ICharacter<TLocomotion>
        where TLocomotion : class, ILocomotion
    {
        public virtual string Name => Transform.name;

        public Animator Animator { get; }

        public Transform Transform { get; }

        public GameObject GameObject => Transform.gameObject;

        public abstract TLocomotion Locomotion { get; }

        ILocomotion ILocomotive.Locomotion => Locomotion;

        protected Character(Animator animator, Transform transform)
        {
            Assert.IsNotNull(animator, "animator != null");
            Assert.IsNotNull(transform, "transform != null");

            Animator = animator;
            Transform = transform;
        }
    }
}