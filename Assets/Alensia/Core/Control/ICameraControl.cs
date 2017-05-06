using Alensia.Core.Camera;

namespace Alensia.Core.Control
{
    public interface ICameraControl : IControl
    {
        ViewSensitivity ViewSensitivity { get; }

        ICameraManager CameraManager { get; }
    }
}