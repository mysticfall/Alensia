using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.Common;
using Malee;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.I18n
{
    public class LocaleService : ManagedMonoBehavior, ILocaleService
    {
        public CultureInfo Locale
        {
            get { return _locale.Value?.ToCulture(); }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _locale.Value = new LanguageTag(value.IetfLanguageTag);
            }
        }

        public CultureInfo FallbackLocale => _fallbackLocale?.ToCulture();

        public IReadOnlyList<CultureInfo> SupportedLocales => _locales?.Select(l => l.ToCulture()).ToList();

        public IObservable<CultureInfo> OnLocaleChange => _locale?.Select(l => l.ToCulture());

        [SerializeField] private LanguageTagReactiveProperty _locale;

        [SerializeField] private LanguageTag _fallbackLocale;

        [SerializeField, Reorderable] private LanguageTagList _locales;

        public LocaleService()
        {
            _fallbackLocale = new LanguageTag("en-US");

            _locales = new LanguageTagList {_fallbackLocale};
            _locale = new LanguageTagReactiveProperty(_fallbackLocale);
        }
    }
}