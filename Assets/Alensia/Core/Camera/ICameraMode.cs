using Alensia.Core.Common;
using Alensia.Core.Geom;

namespace Alensia.Core.Camera
{
    public interface ICameraMode : ITransformable, IActivatable, IValidatable
    {
        UnityEngine.Camera Camera { get; }

        void ResetCamera();
    }
}