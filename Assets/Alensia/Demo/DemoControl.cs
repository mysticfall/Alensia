using Alensia.Core.Control;
using Alensia.Core.Input;
using Alensia.Core.UI;

namespace Alensia.Demo
{
    public class DemoControl : GameControl
    {
        public DemoControl(
            IUIManager uiManager, 
            IInputManager inputManager) : base(uiManager, inputManager)
        {
        }
    }
}