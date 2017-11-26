using Alensia.Core.Locomotion;
using Alensia.Core.Sensor;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Character
{
    public abstract class Humanoid : Character<IBinocularVision, ILeggedLocomotion>, IHumanoid
    {
        public override Transform Head => _head;

        [Inject]
        public override IBinocularVision Vision { get; }

        [Inject]
        public override ILeggedLocomotion Locomotion { get; }

        private Transform _head;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _head = GetBodyPart(HumanBodyBones.Head);
        }

        public Transform GetBodyPart(HumanBodyBones bone) => Animator.GetBoneTransform(bone);
    }
}