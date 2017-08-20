using System;
using System.Globalization;
using Alensia.Core.I18n;
using UniRx;
using UnityEngine;

namespace Alensia.Core.UI
{
    public class EditorUIContext : ScriptableObject, IUIContext
    {
        public LanguageTag Locale = new LanguageTag("en-US");

        public ResourceSettings Resources;

        public ITranslator Translator { get; private set; }

        public IComponent ActiveComponent { get; set; }

        public UniRx.IObservable<CultureInfo> OnLocaleChange => _locale;

        public UniRx.IObservable<IComponent> OnActiveComponentChange => _activeComponent;

        private readonly IReactiveProperty<CultureInfo> _locale;

        private readonly IReactiveProperty<IComponent> _activeComponent;

        public EditorUIContext()
        {
            _locale = new ReactiveProperty<CultureInfo>();
            _activeComponent = new ReactiveProperty<IComponent>();
        }

        private void OnValidate()
        {
            var settings = new LocaleService.Settings
            {
                DefaultLocale = Locale,
                SupportedLocales = new[] {Locale}
            };

            Translator = CreateTranslator(new LocaleService(settings));

            _locale.Value = Locale.ToCulture();
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