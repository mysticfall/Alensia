namespace Alensia.Core.Camera
{
    public interface IThirdPersonCamera : IPerspectiveCamera, IRotatableCamera
    {
        WallAvoidanceSettings WallAvoidanceSettings { get; }
    }
}