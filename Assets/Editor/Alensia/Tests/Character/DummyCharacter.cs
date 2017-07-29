using Alensia.Core.Character.Generic;
using Alensia.Core.Locomotion;
using Alensia.Core.Physics;
using UnityEngine;

namespace Alensia.Tests.Character
{
    public class DummyCharacter : ICharacter<ILeggedLocomotion>
    {
        public string Name => Transform.name;

        public ILeggedLocomotion Locomotion { get; }

        public Animator Animator { get; }

        public Transform Transform { get; }

        public GameObject GameObject => Transform.gameObject;

        public Transform Body { get; }

        ILocomotion ILocomotive.Locomotion => Locomotion;

        public DummyCharacter()
        {
            var root = new GameObject();

            Transform = root.transform;
            Animator = root.AddComponent<Animator>();

            var collider = root.AddComponent<BoxCollider>();
            var detector = new LineCastingGroundDetector(collider);

            Locomotion = new LeggedLocomotion(detector, Animator, Transform);

            var body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            Body = body.transform;

            Body.parent = Transform;
            Body.localPosition = new Vector3(0, 0.5f, 0);
        }
    }
}