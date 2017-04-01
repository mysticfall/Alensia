namespace Alensia.Core.Camera
{
    public interface IZoomableCamera : ICameraMode
    {
        DistanceSettings DistanceSettings { get; }

        float Distance { get; set; }
    }
}