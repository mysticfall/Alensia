using UnityEngine;

namespace Alensia.Core.Camera
{
    public abstract class RotatableCamera : CameraMode, IRotatableCamera
    {
        public abstract RotationalConstraints RotationalConstraints { get; }

        public abstract float Heading { get; set; }

        public abstract float Elevation { get; set; }

        public abstract Vector3 Pivot { get; }

        public abstract Vector3 AxisForward { get; }

        public abstract Vector3 AxisUp { get; }

        public Vector3 AxisRight => Vector3.Cross(AxisUp, AxisForward);

        public override void ResetCamera()
        {
            base.ResetCamera();

            Heading = 0;
            Elevation = 0;
        }
    }
}