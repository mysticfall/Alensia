using Alensia.Core.Common;
using Alensia.Core.Geom;
using Alensia.Core.Interaction;
using UnityEngine;

namespace Alensia.Core.Locomotion
{
    public interface ILocomotion : ITransformable, IActivatable
    {
        float Move(Vector3 direction);

        float MoveTowards(Vector3 position);

        float Rotate(Vector3 axis);

        float RotateTowards(Vector3 axis, float degree);
    }
}