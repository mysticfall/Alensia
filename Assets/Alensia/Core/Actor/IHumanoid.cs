using Alensia.Core.Locomotion;
using UnityEngine;

namespace Alensia.Core.Actor
{
    public interface IHumanoid : IActor
    {
        IWalker Locomotion { get; }

        Transform GetBodyPart(HumanBodyBones bone);
    }
}