using Alensia.Core.Actor;
using Alensia.Core.Camera;

namespace Alensia.Core.Control
{
    public interface IPlayerController<out TPlayer> where TPlayer : IActor
    {
        TPlayer Player { get; }

        IInputManager InputManager { get; }

        ICameraManager CameraManager { get; }

        ViewSensitivity ViewSensitivity { get; }
    }
}