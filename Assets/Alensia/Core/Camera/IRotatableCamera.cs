using Alensia.Core.Geom;

namespace Alensia.Core.Camera
{
    public interface IRotatableCamera : ICameraMode, IDirectional
    {
        RotationalConstraints RotationalConstraints { get; }

        float Heading { get; set; }

        float Elevation { get; set; }
    }
}