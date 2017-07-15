using Alensia.Core.Character;

namespace Alensia.Core.Camera
{
    public interface IFirstPersonCamera : IPerspectiveCamera,
        ITrackingCamera<ICharacter>, IRotatableCamera
    {
    }
}