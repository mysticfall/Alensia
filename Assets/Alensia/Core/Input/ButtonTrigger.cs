using UnityEngine.Assertions;
using static UnityEngine.Input;

namespace Alensia.Core.Input
{
    public class ButtonTrigger : ITrigger
    {
        public readonly string Button;

        public bool Up => GetButtonUp(Button);

        public bool Down => GetButtonDown(Button);

        public bool Hold => GetButton(Button);

        public ButtonTrigger(string button)
        {
            Assert.IsNotNull(button, "button != null");

            Button = button;
        }
    }
}