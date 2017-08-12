using Alensia.Core.Camera;
using Alensia.Core.Character;
using Alensia.Core.Common;
using Alensia.Core.Control;
using Alensia.Core.Game;
using UniRx;
using Zenject;

namespace Alensia.Demo.UMA
{
    public class UMADemo : Game
    {
        public IPlayerController Controller { get; }

        public ICameraManager CamaraManager { get; }

        public UMADemo(
            [Inject(Id = PlayerController.PlayerAliasName)] IReferenceAlias<IHumanoid> player,
            IPlayerController controller,
            ICameraManager camaraManager,
            [InjectOptional] Settings settings) : base(settings)
        {
            Controller = controller;
            CamaraManager = camaraManager;

            player.OnChange.Subscribe(OnPlayerChange).AddTo(this);
        }

        protected virtual void OnPlayerChange(IHumanoid player)
        {
            CamaraManager.Switch<CharacterCamera>().Track(player);

            Controller.DisablePlayerControl();
        }
    }
}