using Alensia.Core.Input;
using UnityEngine.Assertions;

namespace Alensia.Core.UI
{
    public abstract class ToggleUIControl : Control.Control, IUIControl
    {
        public IUIManager UIManager { get; }

        protected ToggleUIControl(
            UIManager uiManager, IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(uiManager, "uiManager != null");

            UIManager = uiManager;
        }
    }
}