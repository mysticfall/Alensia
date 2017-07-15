using Alensia.Core.Character;

namespace Alensia.Core.Camera
{
    public interface IThirdPersonCamera : IPerspectiveCamera, 
        ITrackingCamera<ICharacter>, IRotatableCamera
    {
        WallAvoidanceSettings WallAvoidanceSettings { get; }
    }
}