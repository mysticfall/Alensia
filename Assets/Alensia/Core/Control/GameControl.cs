using System.Collections.Generic;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UnityEngine;

namespace Alensia.Core.Control
{
    public class GameControl : Control
    {
        public const string Id = "Game";

        public IBindingKey<ITriggerInput> ShowMenu = Keys.ShowMenu;

        protected ITriggerInput ShowMenuInput { get; private set; }

        public override bool Valid => base.Valid && ShowMenuInput != null;

        public GameControl(IInputManager inputManager) : base(inputManager)
        {
        }

        protected override ICollection<IBindingKey> PrepareBindings()
        {
            return new List<IBindingKey> {Keys.ShowMenu};
        }


        protected override void OnBindingChange(IBindingKey key)
        {
            base.OnBindingChange(key);

            if (Equals(key, Keys.ShowMenu))
            {
                ShowMenuInput = InputManager.Get(Keys.ShowMenu);
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            Subsribe(ShowMenuInput.Value, OnShowMenu);
        }

        protected void OnShowMenu(float value)
        {
            //TODO: We don't have any menu yet, so we just quit for now.
            Application.Quit();
        }

        public static class Keys
        {
            public static IBindingKey<ITriggerInput> ShowMenu =
                new BindingKey<ITriggerInput>(Id + ".ShowMenu");
        }
    }
}