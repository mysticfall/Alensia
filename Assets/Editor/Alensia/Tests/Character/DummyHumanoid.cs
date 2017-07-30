using Alensia.Core.Character;
using UnityEngine;

namespace Alensia.Tests.Character
{
    public class DummyHumanoid : DummyCharacter, IHumanoid
    {
        public Transform Head { get; }

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