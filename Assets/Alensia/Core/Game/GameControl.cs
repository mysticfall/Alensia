using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using Alensia.Core.UI;
using Alensia.Core.UI.Screen;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Game
{
    public class GameControl : Control.Control
    {
        public const string Category = "General";

        public string MainMenu => _mainMenu;

        [Inject]
        public IUIContext UIContext { get; }

        public IBindingKey<ITriggerInput> ShowMenu = Keys.ShowMenu;

        protected ITriggerInput ShowMenuInput { get; private set; }

        public override bool Valid => base.Valid && ShowMenuInput != null && MainMenu != null;

        protected override IEnumerable<IBindingKey> PrepareBindings() => new List<IBindingKey> {ShowMenu};

        [SerializeField] private string _mainMenu = "MainMenu";

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
                .Subscribe(_ => OnShowMenu(), Debug.LogError)
                .AddTo(disposables);
        }

        protected virtual void OnShowMenu()
        {
            lock (this)
            {
                var screen = (UIContext as IRuntimeUIContext)?.FindScreen(ScreenNames.Windows);
                var menu = screen?.FindUI<IComponentHandler>(MainMenu);

                if (menu == null)
                {
                    screen?.ShowUI<IComponentHandler>(MainMenu);
                }
                else
                {
                    menu.Remove();
                }
            }
        }

        protected virtual void OnHideMenu()
        {
        }

        [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
        public static class Keys
        {
            public static IBindingKey<ITriggerInput> ShowMenu = 
                new BindingKey<ITriggerInput>(Category + ".ShowMenu");
        }
    }
}