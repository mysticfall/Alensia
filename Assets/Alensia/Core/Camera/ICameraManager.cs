using System.Collections.Generic;
using Alensia.Core.Actor;
using UniRx;

namespace Alensia.Core.Camera
{
    public interface ICameraManager
    {
        IReadOnlyReactiveProperty<ICameraMode> Mode { get; }

        IReadOnlyCollection<ICameraMode> AvailableModes { get; }

        T Switch<T>() where T : class, ICameraMode;

        IFirstPersonCamera ToFirstPerson(IActor target);

        IThirdPersonCamera ToThirdPerson(IActor target);
    }
}