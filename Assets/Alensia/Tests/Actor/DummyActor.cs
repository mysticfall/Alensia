using Alensia.Core.Actor;
using Alensia.Core.Locomotion;
using UnityEngine;

namespace Alensia.Tests.Actor
{
    public class DummyActor : IActor
    {
        public IWalker Locomotion { get; private set; }

        public Animator Animator { get; private set; }

        public Transform Transform { get; private set; }

        public Transform Body { get; private set; }

        public DummyActor()
        {
            var root = new GameObject();

            Transform = root.transform;
            Animator = root.AddComponent<Animator>();

            Locomotion = new Walker(Animator, Transform);

            var body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            Body = body.transform;

            Body.parent = Transform;
            Body.localPosition = new Vector3(0, 0.5f, 0);
        }
    }
}