using Alensia.Core.Locomotion;
using Alensia.Core.Sensor;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Character
{
    public class Humanoid : Character<IBinocularVision, ILeggedLocomotion>, IHumanoid
    {
        public override Race Race => _race;

        public override Sex Sex => _sex;

        public override Transform Head => _head;

        [Inject]
        public override IBinocularVision Vision { get; }

        [Inject]
        public override ILeggedLocomotion Locomotion { get; }

        [SerializeField] private Race _race = Race.Human;

        [SerializeField] private Sex _sex = Sex.Male;

        private Transform _head;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _head = GetBodyPart(HumanBodyBones.Head);
        }

        public Transform GetBodyPart(HumanBodyBones bone) => Animator.GetBoneTransform(bone);
    }
}