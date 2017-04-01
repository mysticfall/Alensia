using Alensia.Core.Common;

namespace Alensia.Core.Camera
{
    public interface IThirdPersonCamera : IRotatableCamera
    {
        WallAvoidanceSettings WallAvoidanceSettings { get; }

        void Initialize(ITransformable target);
    }
}