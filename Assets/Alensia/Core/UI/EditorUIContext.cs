using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.I18n;
using Alensia.Core.UI.Cursor;
using Alensia.Core.UI.Screen;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.UI
{
    public class EditorUIContext : ScriptableObject, IUIContext
    {
        public ResourceSettings Resources;

        public UIStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }

        public CultureInfo Locale => _locale?.ToCulture();

        public ITranslator Translator { get; private set; }

        public DiContainer DiContainer => null;

        public CursorState CursorState { get; set; }

        public string DefaultCursor { get; set; }

        public IInteractableComponent ActiveComponent { get; set; }

        public IObservable<UIStyle> OnStyleChange => _styleProperty;

        public IObservable<CultureInfo> OnLocaleChange => _localeProperty;

        public IObservable<IInteractableComponent> OnActiveComponentChange => _activeComponentProperty;

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

        public void Initialize()
        {
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

        public T Instantiate<T>(GameObject item, Transform parent) where T : IUIElement
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IScreen> Screens => Enumerable.Empty<IScreen>().ToList();

        public IScreen FindScreen(string screen) => null;

        public void ShowScreen(string screen)
        {
        }

        public void HideScreen(string screen)
        {
        }
    }
}