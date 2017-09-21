using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Common;
using UniRx;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Control
{
    public class PlayerController : Controller, IPlayerController
    {
        public const string PlayerAliasName = "Player";

        public IHumanoid Player => PlayerAlias.Reference;

        public IReferenceAlias<IHumanoid> PlayerAlias { get; }

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

        public PlayerController(
            [Inject(Id = PlayerAliasName)] IReferenceAlias<IHumanoid> player,
            IList<IControl> controls) : base(controls)
        {
        }

        [Inject]
        public PlayerController(
            [InjectOptional] Settings settings,
            [Inject(Id = PlayerAliasName)] IReferenceAlias<IHumanoid> player,
            IList<IControl> controls) : base(settings, controls)
        {
            Assert.IsNotNull(player, "player != null");

            _enabled = new ReactiveProperty<bool>(true);

            PlayerAlias = player;
            PlayerAlias.OnChange.Subscribe(OnPlayerChange).AddTo(this);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            OnPlayerControlStateChange
                .Subscribe(v => PlayerControls.ToList().ForEach(c => c.Active = v))
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