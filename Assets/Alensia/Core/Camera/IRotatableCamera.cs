using UnityEngine;

namespace Alensia.Core.Camera
{
    public interface IRotatableCamera : ICameraMode
    {
        RotationalConstraints RotationalConstraints { get; }

        float Heading { get; set; }

        float Elevation { get; set; }

        Vector3 Pivot { get; }

        Vector3 AxisForward { get; }

        Vector3 AxisUp { get; }
    }
}