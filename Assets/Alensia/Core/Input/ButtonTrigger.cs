using UnityEngine.Assertions;

namespace Alensia.Core.Input
{
    public class ButtonTrigger : ITrigger
    {
        public readonly string Button;

        public bool Up
        {
            get { return UnityEngine.Input.GetButtonUp(Button); }
        }

        public bool Down
        {
            get { return UnityEngine.Input.GetButtonDown(Button); }
        }

        public bool Hold
        {
            get { return UnityEngine.Input.GetButton(Button); }
        }

        public ButtonTrigger(string button)
        {
            Assert.IsNotNull(button, "button != null");

            Button = button;
        }
    }
}