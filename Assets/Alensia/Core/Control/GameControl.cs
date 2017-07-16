using System;
using System.Collections.Generic;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using Alensia.Core.UI;
using Alensia.Core.UI.Screen;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Control
{
    public class GameControl : Control
    {
        public const string Category = "General";

        public string MainMenu { get; set; } = "MainMenu";

        public IUIManager UIManager { get; }

        public IBindingKey<ITriggerInput> ShowMenu = Keys.ShowMenu;

        protected ITriggerInput ShowMenuInput { get; private set; }

        public override bool Valid => base.Valid && ShowMenuInput != null && MainMenu != null;

        public GameControl(
            IUIManager uiManager,
            IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(uiManager, "uiManager != null");

            UIManager = uiManager;
        }

        protected override ICollection<IBindingKey> PrepareBindings() => new List<IBindingKey> {ShowMenu};

        protected override void RegisterDefaultBindings()
        {
            base.RegisterDefaultBindings();

            InputManager.Register(
                ShowMenu,
                new TriggerDownInput(new ButtonTrigger("Cancel")));
        }

        protected override void OnBindingChange(IBindingKey key)
        {
            base.OnBindingChange(key);

            if (Equals(key, ShowMenu))
            {
                ShowMenuInput = InputManager.Get(ShowMenu);
            }
        }

        protected override void Subscribe(ICollection<IDisposable> disposables)
        {
            ShowMenuInput?.OnChange
                .Where(_ => Valid)
                .Subscribe(_ => OnShowMenu())
                .AddTo(disposables);
        }

        protected virtual void OnShowMenu()
        {
            lock (this)
            {
                var screen = UIManager.FindScreen(ScreenNames.Windows);
                var menu = screen.FindUI<IUIHandler>(MainMenu);

                if (menu == null)
                {
                    screen.ShowUI<UIHandler>(MainMenu);
                }
                else
                {
                    menu.Close();
                }
            }
        }

        protected virtual void OnHideMenu()
        {
        }

        public static class Keys
        {
            public static IBindingKey<ITriggerInput> ShowMenu =
                new BindingKey<ITriggerInput>(Category + ".ShowMenu");
        }
    }
}