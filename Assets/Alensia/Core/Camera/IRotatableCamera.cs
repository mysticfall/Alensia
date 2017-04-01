using UnityEngine;

namespace Alensia.Core.Camera
{
    public interface IRotatableCamera : ICameraMode
    {
        RotationalConstraints RotationalConstraints { get; }

        float Heading { get; set; }

        float Elevation { get; set; }

        Transform Pivot { get; }
    }
}