using System;
using System.Collections.Generic;
using Alensia.Core.Character;
using UniRx;

namespace Alensia.Core.Control
{
    public interface IPlayerController : IController
    {
        IHumanoid Player { get; }

        CharacterAlias PlayerAlias { get; }

        IReadOnlyList<IPlayerControl> PlayerControls { get; }

        bool PlayerControlEnabled { get; set; }

        IObservable<Unit> OnEnablePlayerControl { get; }

        IObservable<Unit> OnDisablePlayerControl { get; }

        IObservable<bool> OnPlayerControlStateChange { get; }

        void EnablePlayerControl();

        void DisablePlayerControl();
    }
}