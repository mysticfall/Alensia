using System;
using System.Globalization;
using Alensia.Core.I18n;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using UEObject = UnityEngine.Object;

namespace Alensia.Core.UI
{
    public class UIContext : IUIContext
    {
        public UIStyle Style => _style.Value;

        public CultureInfo Locale => Translator?.LocaleService?.CurrentLocale;

        public ITranslator Translator { get; }

        public IInteractableComponent ActiveComponent
        {
            get { return _activeComponent.Value; }
            set { _activeComponent.Value = value; }
        }

        public UniRx.IObservable<UIStyle> OnStyleChange => _style;

        public UniRx.IObservable<CultureInfo> OnLocaleChange => Translator.LocaleService.OnLocaleChange;

        public UniRx.IObservable<IInteractableComponent> OnActiveComponentChange => _activeComponent;

        protected DiContainer DiContainer { get; }

        private readonly IReadOnlyReactiveProperty<UIStyle> _style;

        private readonly IReactiveProperty<IInteractableComponent> _activeComponent;

        public UIContext(
            IReadOnlyReactiveProperty<UIStyle> style,
            ITranslator translator,
            DiContainer container)
        {
            Assert.IsNotNull(style, "style != null");
            Assert.IsNotNull(translator, "translator != null");
            Assert.IsNotNull(container, "container != null");

            Translator = translator;
            DiContainer = container;

            _style = style;
            _activeComponent = new ReactiveProperty<IInteractableComponent>();
        }

        public virtual TUI Instantiate<TUI>(GameObject item, Transform parent)
            where TUI : IUIElement
        {
            return Instantiate<TUI>(item, parent, null);
        }

        public virtual TUI Instantiate<TDef, TUI>(TDef definition, Transform parent)
            where TDef : IUIDefinition
            where TUI : IUIElement
        {
            Func<TUI, TUI> beforeInitialize = u =>
            {
                u.GameObject.name = definition.Name;
                u.Visible = definition.Enable;

                return u;
            };

            return Instantiate(definition.Item, parent, beforeInitialize);
        }

        protected virtual TUI Instantiate<TUI>(
            GameObject item, Transform parent, Func<TUI, TUI> beforeInitialize)
            where TUI : IUIElement
        {
            TUI ui;

            if (item.scene != parent.gameObject.scene)
            {
                ui = UEObject.Instantiate(item, parent).GetComponent<TUI>();
            }
            else
            {
                ui = item.GetComponent<TUI>();

                if (item.transform.parent != parent)
                {
                    item.transform.SetParent(parent);
                }
            }

            if (ui == null)
            {
                throw new ArgumentException(
                    $"Unable to find a suitable component on object: '{item.name}'.");
            }

            var handler = ui as IComponentHandler ?? ui.GameObject.GetComponent<IComponentHandler>();

            if (handler != null)
            {
                DiContainer.Inject(handler);
            }

            beforeInitialize?.Invoke(ui);

            ui.Initialize(this);

            return ui;
        }
    }
}