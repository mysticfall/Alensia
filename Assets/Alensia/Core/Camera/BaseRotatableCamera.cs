using UnityEngine;

namespace Alensia.Core.Camera
{
    public abstract class BaseRotatableCamera : BaseCameraMode, IRotatableCamera
    {
        public abstract RotationalConstraints RotationalConstraints { get; }

        public abstract float Heading { get; set; }

        public abstract float Elevation { get; set; }

        public abstract Vector3 Pivot { get; }

        public abstract Vector3 AxisForward { get; }

        public abstract Vector3 AxisUp { get; }

        public Vector3 AxisRight
        {
            get { return Vector3.Cross(AxisUp, AxisForward); }
        }

        protected BaseRotatableCamera(UnityEngine.Camera camera) : base(camera)
        {
        }
    }
}