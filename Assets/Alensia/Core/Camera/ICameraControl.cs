using Alensia.Core.Control;

namespace Alensia.Core.Camera
{
    public interface ICameraControl : IControl
    {
        ICameraManager CameraManager { get; }
    }
}