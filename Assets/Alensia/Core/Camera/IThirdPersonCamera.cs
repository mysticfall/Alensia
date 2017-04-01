namespace Alensia.Core.Camera
{
    public interface IThirdPersonCamera : ITrackingCamera, IRotatableCamera
    {
        WallAvoidanceSettings WallAvoidanceSettings { get; }
    }
}