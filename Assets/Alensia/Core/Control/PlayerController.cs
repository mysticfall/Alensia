using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Control
{
    public class PlayerController : Controller, IPlayerController
    {
        public const string PlayerAliasName = "Player";

        public IHumanoid Player => PlayerAlias.Reference;

        [Inject(Id = PlayerAliasName)] 
        public CharacterAlias PlayerAlias { get; }

        public IReadOnlyList<IPlayerControl> PlayerControls =>
            Controls.Select(c => c as IPlayerControl).Where(c => c != null).ToList();

        public bool PlayerControlEnabled
        {
            get { return _enabled.Value; }
            set { _enabled.Value = value; }
        }

        public IObservable<Unit> OnEnablePlayerControl => _enabled.Where(s => s).AsUnitObservable();

        public IObservable<Unit> OnDisablePlayerControl => _enabled.Where(s => !s).AsUnitObservable();

        public IObservable<bool> OnPlayerControlStateChange => _enabled;

        private readonly IReactiveProperty<bool> _enabled;

        public PlayerController()
        {
            _enabled = new ReactiveProperty<bool>(true);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            PlayerAlias.OnChange
                .Subscribe(OnPlayerChange, Debug.LogError)
                .AddTo(this);

            OnPlayerControlStateChange
                .Subscribe(v => PlayerControls.ToList().ForEach(c => c.Active = v), Debug.LogError)
                .AddTo(this);
        }

        public void EnablePlayerControl() => PlayerControlEnabled = true;

        public void DisablePlayerControl() => PlayerControlEnabled = false;

        protected virtual void OnPlayerChange(IHumanoid player)
        {
            foreach (var control in PlayerControls)
            {
                control.Player = player;
            }
        }
    }
}