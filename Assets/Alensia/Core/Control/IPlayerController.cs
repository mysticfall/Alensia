using System.Collections.Generic;
using Alensia.Core.Actor;
using Alensia.Core.Common;
using UniRx;

namespace Alensia.Core.Control
{
    public interface IPlayerController : IController
    {
        IHumanoid Player { get; }

        IReferenceAlias<IHumanoid> PlayerAlias { get; }

        IReadOnlyList<IPlayerControl> PlayerControls { get; }

        bool PlayerControlEnabled { get; set; }

        IObservable<Unit> OnEnablePlayerControl { get; }

        IObservable<Unit> OnDisablePlayerControl { get; }

        IObservable<bool> OnPlayerControlStateChange { get; }

        void EnablePlayerControl();

        void DisablePlayerControl();
    }
}