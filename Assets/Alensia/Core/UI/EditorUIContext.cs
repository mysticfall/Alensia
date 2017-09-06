using System;
using System.Globalization;
using Alensia.Core.I18n;
using UniRx;
using UnityEngine;

namespace Alensia.Core.UI
{
    public class EditorUIContext : ScriptableObject, IUIContext
    {
        public ResourceSettings Resources;

        public UIStyle Style => _style;

        public CultureInfo Locale => _locale?.ToCulture();

        public ITranslator Translator { get; private set; }

        public IInteractableComponent ActiveComponent { get; set; }

        public UniRx.IObservable<UIStyle> OnStyleChange => _styleProperty;

        public UniRx.IObservable<CultureInfo> OnLocaleChange => _localeProperty;

        public UniRx.IObservable<IInteractableComponent> OnActiveComponentChange => _activeComponentProperty;

        [SerializeField] private UIStyle _style;

        [SerializeField] private LanguageTag _locale = new LanguageTag("en-US");

        private readonly IReactiveProperty<UIStyle> _styleProperty;

        private readonly IReactiveProperty<CultureInfo> _localeProperty;

        private readonly IReactiveProperty<IInteractableComponent> _activeComponentProperty;

        public EditorUIContext()
        {
            _styleProperty = new ReactiveProperty<UIStyle>();
            _localeProperty = new ReactiveProperty<CultureInfo>();
            _activeComponentProperty = new ReactiveProperty<IInteractableComponent>();
        }

        internal void RefreshStyle()
        {
            var style = Style;

            _styleProperty.Value = null;
            _styleProperty.Value = style;
        }

        private void OnValidate()
        {
            var settings = new LocaleService.Settings
            {
                DefaultLocale = _locale,
                SupportedLocales = new[] {_locale}
            };

            Translator = CreateTranslator(new LocaleService(settings));

            _localeProperty.Value = Locale;
            _styleProperty.Value = Style;

            if (Style != null)
            {
                Style.EditorUIContext = this;
            }
        }

        protected virtual ITranslator CreateTranslator(ILocaleService localeService)
        {
            var translator = new JsonResourceTranslator(Resources, localeService);

            translator.Initialize();

            return translator;
        }

        public TUI Instantiate<TDef, TUI>(TDef definition, Transform parent)
            where TDef : IUIDefinition where TUI : IUIElement
        {
            throw new NotImplementedException();
        }

        public TUI Instantiate<TUI>(GameObject item, Transform parent) where TUI : IUIElement
        {
            throw new NotImplementedException();
        }
    }
}