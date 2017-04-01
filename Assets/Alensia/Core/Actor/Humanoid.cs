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
            Transform transform,
            Animator animator) : base(transform, animator)
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