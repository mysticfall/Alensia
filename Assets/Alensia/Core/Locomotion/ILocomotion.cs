using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Locomotion
{
    public interface ILocomotion : ITransformable
    {
        Vector3 Move(Vector3 velocity);

        Vector3 Rotate(Vector3 velocity);
    }
}