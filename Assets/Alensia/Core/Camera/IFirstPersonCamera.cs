using Alensia.Core.Actor;

namespace Alensia.Core.Camera
{
    public interface IFirstPersonCamera : IRotatableCamera
    {
        void Initialize(IHumanoid target);
    }
}