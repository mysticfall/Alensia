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

        public CultureInfo Locale => _locale?.ToCulture();

        public ITranslator Translator { get; private set; }

        public IComponent ActiveComponent { get; set; }

        public UniRx.IObservable<CultureInfo> OnLocaleChange => _localeProperty;

        public UniRx.IObservable<IComponent> OnActiveComponentChange => _activeComponentProperty;

        [SerializeField] private LanguageTag _locale = new LanguageTag("en-US");

        private readonly IReactiveProperty<CultureInfo> _localeProperty;

        private readonly IReactiveProperty<IComponent> _activeComponentProperty;

        public EditorUIContext()
        {
            _localeProperty = new ReactiveProperty<CultureInfo>();
            _activeComponentProperty = new ReactiveProperty<IComponent>();
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