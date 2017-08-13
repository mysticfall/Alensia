using Alensia.Core.Camera;
using Alensia.Core.Character;
using Alensia.Core.Common;
using Alensia.Core.Control;
using UniRx;
using Zenject;

namespace Alensia.Demo.UMA
{
    public class UMADemoScene : BaseObject
    {
        public IPlayerController Controller { get; }

        public ICameraManager CamaraManager { get; }

        public UMADemoScene(
            [Inject(Id = PlayerController.PlayerAliasName)] IReferenceAlias<IHumanoid> player,
            IPlayerController controller,
            ICameraManager camaraManager)
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