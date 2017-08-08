using Alensia.Core.Locomotion;
using Alensia.Core.Sensor;
using UnityEngine;

namespace Alensia.Core.Character.Morph
{
    public class MorphableHumanoid : Humanoid, IMorphable
    {
        public IMorphSet Morphs { get; }

        public override Race Race => Morphs.Race;

        public override Sex Sex => Morphs.Sex;

        public MorphableHumanoid(
            IMorphSet morphs,
            IBinocularVision vision,
            ILeggedLocomotion locomotion,
            Animator animator,
            Transform transform) : base(
            morphs.Race, morphs.Sex, vision, locomotion, animator, transform)
        {
            Morphs = morphs;
        }
    }
}