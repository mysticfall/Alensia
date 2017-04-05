using Alensia.Core.Locomotion;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Actor
{
    public class Humanoid : Actor, IHumanoid
    {
        public IWalker Locomotion { get; private set; }

        public Humanoid(
            IWalker locomotion,
            Animator animator,
            Transform transform) : base(animator, transform)
        {
            Assert.IsNotNull(locomotion, "locomotion != null");

            Locomotion = locomotion;
        }

        public Transform GetBodyPart(HumanBodyBones bone)
        {
            return Animator.GetBoneTransform(bone);
        }
    }
}