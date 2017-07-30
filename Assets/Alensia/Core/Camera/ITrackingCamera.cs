using Alensia.Core.Geom;

namespace Alensia.Core.Camera
{
    public interface ITrackingCamera<T> : ICameraMode where T : ITransformable
    {
        T Target { get; }

        void Track(T target);
    }
}