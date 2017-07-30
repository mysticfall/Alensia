using Alensia.Core.Character.Generic;
using Alensia.Core.Locomotion;
using Alensia.Core.Sensor;
using UnityEngine;

namespace Alensia.Core.Character
{
    public interface IHumanoid : ICharacter<IBinocularVision, ILeggedLocomotion>
    {
        Transform Head { get; }

        Transform GetBodyPart(HumanBodyBones bone);
    }
}