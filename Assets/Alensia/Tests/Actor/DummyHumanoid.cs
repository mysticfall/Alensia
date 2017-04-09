using Alensia.Core.Actor;
using UnityEngine;

namespace Alensia.Tests.Actor
{
    public class DummyHumanoid : DummyActor, IHumanoid
    {
        public Transform Head { get; private set; }

        public Transform LeftEye
        {
            get { return null; }
        }

        public Transform RightEye
        {
            get { return null; }
        }

        public Vector3 Viewpoint
        {
            get { return Head.position; }
        }

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