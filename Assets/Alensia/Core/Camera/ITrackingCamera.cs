using Alensia.Core.Common;

namespace Alensia.Core.Camera
{
    public interface ITrackingCamera : ICameraMode
    {
        ITransformable Target { get; }

        void Initialize(ITransformable target);
    }
}