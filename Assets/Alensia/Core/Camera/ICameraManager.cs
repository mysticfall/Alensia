using System;
using System.Collections.Generic;
using Alensia.Core.Character;
using UniRx;

namespace Alensia.Core.Camera
{
    public interface ICameraManager : IFocusTracking
    {
        ICameraMode Mode { get; set; }

        IEnumerable<ICameraMode> AvailableModes { get; }

        IObservable<ICameraMode> OnCameraModeChange { get; }

        IObservable<Unit> OnCameraUpdate{ get; }

        T Switch<T>() where T : class, ICameraMode;

        IFirstPersonCamera ToFirstPerson(ICharacter target);

        IThirdPersonCamera ToThirdPerson(ICharacter target);
    }
}