using System.Collections.Generic;
using Alensia.Core.Character;
using UniRx;

namespace Alensia.Core.Camera
{
    public interface ICameraManager
    {
        ICameraMode Mode { get; set; }

        IReadOnlyCollection<ICameraMode> AvailableModes { get; }

        T Switch<T>() where T : class, ICameraMode;

        IFirstPersonCamera ToFirstPerson(ICharacter target);

        IThirdPersonCamera ToThirdPerson(ICharacter target);

        IObservable<ICameraMode> OnCameraModeChange { get; }
    }
}