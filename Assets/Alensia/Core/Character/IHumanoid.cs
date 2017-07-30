using Alensia.Core.Locomotion;
using UnityEngine;

namespace Alensia.Core.Character
{
    public interface IHumanoid : ICharacter
    {
        Transform Head { get; }

        Transform LeftEye { get; }

        Transform RightEye { get; }

        Vector3 Viewpoint { get; }

        ILeggedLocomotion Locomotion { get; }

        Transform GetBodyPart(HumanBodyBones bone);
    }
}