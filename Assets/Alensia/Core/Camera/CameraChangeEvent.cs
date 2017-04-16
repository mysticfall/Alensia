using Zenject;

namespace Alensia.Core.Camera
{
    public class CameraChangeEvent : Signal<ICameraMode, CameraChangeEvent>
    {
    }
}