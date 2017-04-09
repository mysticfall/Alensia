using Alensia.Core.Locomotion;
using UnityEngine;

namespace Alensia.Core.Actor
{
    public interface IHumanoid : IActor
    {
        Transform Head { get; }

        Transform LeftEye { get; }

        Transform RightEye { get; }

        Vector3 Viewpoint { get; }

        IWalker Locomotion { get; }

        Transform GetBodyPart(HumanBodyBones bone);
    }
}