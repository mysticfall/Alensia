using Alensia.Core.Locomotion;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Actor
{
    public class Humanoid : Actor, IHumanoid
    {
        public Transform Head { get; private set; }

        public Transform LeftEye { get; private set; }

        public Transform RightEye { get; private set; }

        public Vector3 Viewpoint
        {
            get
            {
                if (LeftEye && RightEye)
                {
                    return (LeftEye.position + RightEye.position) / 2;
                }

                return Head ? Head.position : Transform.position;
            }
        }

        public IWalkingLocomotion Locomotion { get; private set; }

        public Humanoid(
            IWalkingLocomotion locomotion,
            Animator animator,
            Transform transform) : base(animator, transform)
        {
            Assert.IsNotNull(locomotion, "locomotion != null");

            Head = GetBodyPart(HumanBodyBones.Head);

            LeftEye = GetBodyPart(HumanBodyBones.LeftEye);
            RightEye = GetBodyPart(HumanBodyBones.RightEye);

            Locomotion = locomotion;
        }

        public Transform GetBodyPart(HumanBodyBones bone)
        {
            return Animator.GetBoneTransform(bone);
        }
    }
}