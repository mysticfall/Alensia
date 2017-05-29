using System.Collections.Generic;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using Alensia.Core.UI;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Control
{
    public abstract class GameControl : Control
    {
        public const string Id = "Game";

        public IUIManager UIManager { get; }

        public IBindingKey<ITriggerInput> ShowMenu = Keys.ShowMenu;

        protected ITriggerInput ShowMenuInput { get; private set; }

        public override bool Valid => base.Valid && ShowMenuInput != null;

        private IComponent _mainMenu;

        protected GameControl(IUIManager uiManager, IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(uiManager, "uiManager != null");

            UIManager = uiManager;
        }

        public override void Initialize()
        {
            base.Initialize();

            UIManager.ComponentRemoved
                .Where(c => _mainMenu == c)
                .Subscribe(_ => _mainMenu = null)
                .AddTo(ConstantObservers);
        }

        protected override ICollection<IBindingKey> PrepareBindings()
        {
            return new List<IBindingKey> {ShowMenu};
        }

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

        protected override void OnActivate()
        {
            base.OnActivate();

            ShowMenuInput?.Value
                .Where(_ => Active && Valid)
                .Subscribe(_ => OnShowMenu())
                .AddTo(Observers);
        }

        protected virtual void OnShowMenu()
        {
            lock (this)
            {
                if (_mainMenu != null)
                {
                    UIManager.Remove(_mainMenu);
                }
                else
                {
                    _mainMenu = CreateMainMenu();

                    UIManager.Add(_mainMenu);
                }
            }
        }

        protected abstract IComponent CreateMainMenu();

        public static class Keys
        {
            public static IBindingKey<ITriggerInput> ShowMenu =
                new BindingKey<ITriggerInput>(Id + ".ShowMenu");
        }
    }
}