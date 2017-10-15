namespace Alensia.Core.Camera
{
    public interface IFocusTrackingCamera : ICameraMode, IFocusTracking
    {
        FocusSettings FocusSettings { get; }
    }
}