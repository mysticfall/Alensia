using Alensia.Core.Geom;

namespace Alensia.Core.Camera
{
    public interface ITrackingCamera<T> : IUpdatableCamera where T : ITransformable
    {
        T Target { get; }

        void Track(T target);
    }
}