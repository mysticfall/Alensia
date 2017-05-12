using Alensia.Core.Actor;
using UnityEngine;

namespace Alensia.Tests.Actor
{
    public class DummyHumanoid : DummyActor, IHumanoid
    {
        public Transform Head { get; }

        public Transform LeftEye => null;

        public Transform RightEye => null;

        public Vector3 Viewpoint => Head.position;

        public DummyHumanoid()
        {
            var head = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            head.transform.parent = Body;
            head.transform.localPosition = new Vector3(0, 1.5f, 0);

            Head = head.transform;
        }

        public Transform GetBodyPart(HumanBodyBones bone)
        {
            return bone == HumanBodyBones.Head ? Head : Animator.GetBoneTransform(bone);
        }
    }
}