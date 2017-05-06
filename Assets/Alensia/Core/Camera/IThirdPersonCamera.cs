namespace Alensia.Core.Camera
{
    public interface IThirdPersonCamera : IPerspectiveCamera, ITrackingCamera, IRotatableCamera
    {
        WallAvoidanceSettings WallAvoidanceSettings { get; }
    }
}