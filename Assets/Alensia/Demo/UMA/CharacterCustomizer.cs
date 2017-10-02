using Alensia.Core.Control;
using Alensia.Core.UI;
using Zenject;

namespace Alensia.Demo.UMA
{
    public class CharacterCustomizer : ComponentHandler<Panel>
    {
        [Inject]
        public IPlayerController Controller { get; }

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            Controller.DisablePlayerControl();
        }
    }
}