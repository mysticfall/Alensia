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
        public ITranslator Translator { get; }

        public IComponent ActiveComponent
        {
            get { return _activeComponent.Value; }
            set { _activeComponent.Value = value; }
        }

        public UniRx.IObservable<CultureInfo> OnLocaleChange => Translator.LocaleService.OnLocaleChange;

        public UniRx.IObservable<IComponent> OnActiveComponentChange => _activeComponent;

        protected DiContainer DiContainer { get; }

        private readonly IReactiveProperty<IComponent> _activeComponent;

        public UIContext(ITranslator translator, DiContainer container)
        {
            Assert.IsNotNull(translator, "translator != null");
            Assert.IsNotNull(container, "container != null");

            Translator = translator;
            DiContainer = container;

            _activeComponent = new ReactiveProperty<IComponent>();
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