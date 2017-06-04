using Alensia.Core.Control;
using Alensia.Core.I18n;
using Alensia.Core.Input;
using Alensia.Core.UI;
using UnityEngine.Assertions;

namespace Alensia.Demo
{
    public class DemoControl : GameControl
    {
        private readonly ITranslator _translator;

        public DemoControl(
            ITranslator translator,
            IUIManager uiManager,
            IInputManager inputManager) : base(uiManager, inputManager)
        {
            Assert.IsNotNull(translator, "translator != null");

            _translator = translator;
        }

        protected override IComponent CreateMainMenu() => new MainMenu(_translator, UIManager);
    }
}