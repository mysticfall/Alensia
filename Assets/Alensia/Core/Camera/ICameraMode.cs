using Alensia.Core.Common;

namespace Alensia.Core.Camera
{
    public interface ICameraMode : ITransformable, IActivatable, IValidatable
    {
        UnityEngine.Camera Camera { get; }
    }
}