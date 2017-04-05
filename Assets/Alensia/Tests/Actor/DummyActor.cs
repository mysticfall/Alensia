using Alensia.Core.Actor;
using Alensia.Core.Locomotion;
using UnityEngine;

namespace Alensia.Tests.Actor
{
    public class DummyActor : IHumanoid
    {
        public IWalker Locomotion { get; private set; }

        public Animator Animator { get; private set; }

        public Transform Transform { get; private set; }

        public Transform Head { get; private set; }

        public DummyActor()
        {
            var root = new GameObject();

            Transform = root.transform;
            Animator = root.AddComponent<Animator>();

            var locomotion = new TransformDrivenLocomotion(Animator, Transform);

            Locomotion = new Walker(locomotion);

            var body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            body.transform.parent = Transform;
            body.transform.localPosition = new Vector3(0, 0.5f, 0);


            var head = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            head.transform.parent = body.transform;
            head.transform.localPosition = new Vector3(0, 1.5f, 0);

            Head = head.transform;
        }

        public Transform GetBodyPart(HumanBodyBones bone)
        {
            return bone == HumanBodyBones.Head ? Head : Animator.GetBoneTransform(bone);
        }
    }
}