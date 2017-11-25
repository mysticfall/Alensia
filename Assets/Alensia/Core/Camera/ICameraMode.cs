using Alensia.Core.Common;
using Alensia.Core.Geom;
using Alensia.Core.Interaction;

namespace Alensia.Core.Camera
{
    public interface ICameraMode : ITransformable, IActivatable, IValidatable
    {
        UnityEngine.Camera Camera { get; }

        void ResetCamera();
    }
}