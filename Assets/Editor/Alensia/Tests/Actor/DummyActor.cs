using Alensia.Core.Actor;
using Alensia.Core.Locomotion;
using Alensia.Core.Physics;
using UnityEngine;

namespace Alensia.Tests.Actor
{
    public class DummyActor : IActor
    {
        public IWalkingLocomotion Locomotion { get; }

        public Animator Animator { get; }

        public Transform Transform { get; }

        public GameObject GameObject => Transform.gameObject;

        public Transform Body { get; }

        public DummyActor()
        {
            var root = new GameObject();

            Transform = root.transform;
            Animator = root.AddComponent<Animator>();

            var collider = root.AddComponent<BoxCollider>();
            var detector = new LineCastingGroundDetector(
                collider, new GroundHitEvent(), new GroundLeaveEvent());

            Locomotion = new WalkingLocomotion(detector, Animator, Transform, new PacingChangeEvent());

            var body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            Body = body.transform;

            Body.parent = Transform;
            Body.localPosition = new Vector3(0, 0.5f, 0);
        }
    }
}