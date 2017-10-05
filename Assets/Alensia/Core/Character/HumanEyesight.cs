using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Character
{
    public class HumanEyesight : Eyesight
    {
        public override Transform LeftEye => _leftEye;

        public override Transform RightEye => _rightEye;

        private Transform _leftEye;

        private Transform _rightEye;

        [Inject]
        public Animator Animator { get; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var leftEye = Animator.GetBoneTransform(HumanBodyBones.LeftEye);
            var rightEye = Animator.GetBoneTransform(HumanBodyBones.RightEye);

            Assert.IsNotNull(leftEye, "leftEye != null");
            Assert.IsNotNull(rightEye, "rightEye != null");

            _leftEye = leftEye;
            _rightEye = rightEye;
        }
    }
}