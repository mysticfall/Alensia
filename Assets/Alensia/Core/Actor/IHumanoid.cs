using Alensia.Core.Locomotion;
using UnityEngine;

namespace Alensia.Core.Actor
{
    public interface IHumanoid : IActor, ILocomotiveObject<IWalker>
    {
        Transform Head { get; }

        Transform LeftEye { get; }

        Transform RightEye { get; }

        Vector3 Viewpoint { get; }

        Transform GetBodyPart(HumanBodyBones bone);
    }
}