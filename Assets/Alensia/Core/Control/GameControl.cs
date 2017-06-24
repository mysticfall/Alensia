using System;
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

        public override string Name => Id;

        public IUIManager UIManager { get; }

        public IBindingKey<ITriggerInput> ShowMenu = Keys.ShowMenu;

        protected ITriggerInput ShowMenuInput { get; private set; }

        public override bool Valid => base.Valid && ShowMenuInput != null;

        protected GameControl(IUIManager uiManager, IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(uiManager, "uiManager != null");

            UIManager = uiManager;
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

        protected override void Subscribe(ICollection<IDisposable> disposables)
        {
            ShowMenuInput?.Value
                .Where(_ => Valid)
                .Subscribe(_ => OnShowMenu())
                .AddTo(disposables);
        }

        protected virtual void OnShowMenu()
        {
            throw new NotImplementedException();
        }

        public static class Keys
        {
            public static IBindingKey<ITriggerInput> ShowMenu =
                new BindingKey<ITriggerInput>(Id + ".ShowMenu");
        }
    }
}