using System;
using System.Globalization;
using Alensia.Core.I18n;
using UniRx;
using UnityEngine;

namespace Alensia.Core.UI
{
    public class EditorUIContext : ScriptableObject, IUIContext
    {
        public UIStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }

        public CultureInfo Locale => _locale?.ToCulture();

        public ITranslator Translator => null;

        public IObservable<UIStyle> OnStyleChange => _styleProperty;

        public IObservable<CultureInfo> OnLocaleChange => _localeProperty?.Select(l => l?.ToCulture());

        [SerializeField] private UIStyle _style;

        [SerializeField] private LanguageTag _locale;

        private readonly IReactiveProperty<UIStyle> _styleProperty;

        private readonly IReactiveProperty<LanguageTag> _localeProperty;

        public EditorUIContext()
        {
            _styleProperty = new UIStyleReactiveProperty();
            _localeProperty = new LanguageTagReactiveProperty(_locale);
        }

        internal void RefreshStyle()
        {
            var style = Style;

            _styleProperty.Value = style;
        }

        private void OnValidate()
        {
            _styleProperty.Value = _style;
            _localeProperty.Value = _locale;

            if (Style != null)
            {
                Style.EditorUIContext = this;
            }
        }
    }
}