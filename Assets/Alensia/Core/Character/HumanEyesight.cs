using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Character
{
    public class HumanEyesight : Eyesight
    {
        public override Transform LeftEye { get; }

        public override Transform RightEye { get; }

        public HumanEyesight(Animator animator)
        {
            Assert.IsNotNull(animator, "animator != null");

            var leftEye = animator.GetBoneTransform(HumanBodyBones.LeftEye);
            var rightEye = animator.GetBoneTransform(HumanBodyBones.RightEye);

            Assert.IsNotNull(leftEye, "leftEye != null");
            Assert.IsNotNull(rightEye, "rightEye != null");

            LeftEye = leftEye;
            RightEye = rightEye;
        }
    }
}