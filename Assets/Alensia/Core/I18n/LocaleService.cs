using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.I18n
{
    public class LocaleService : ILocaleService
    {
        public IReadOnlyList<CultureInfo> SupportedLocales { get; }

        public CultureInfo DefaultLocale { get; }

        public CultureInfo CurrentLocale
        {
            get { return _currentLocale; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                lock (this)
                {
                    if (Equals(_currentLocale, value)) return;

                    _currentLocale = value;
                }

                LocaleChanged.Fire(value);
            }
        }

        public LocaleChangeEvent LocaleChanged { get; }

        private CultureInfo _currentLocale;

        public LocaleService(LocaleChangeEvent localeChanged) : this(new Settings(), localeChanged)
        {
        }

        [Inject]
        public LocaleService(Settings settings, LocaleChangeEvent localeChanged)
        {
            Assert.IsNotNull(localeChanged, "localeChanged != null");
            Assert.IsNotNull(settings, "settings != null");

            Assert.IsNotNull(settings.SupportedLocales, "settings.SupportedLocales != null");

            Assert.IsTrue(
                settings.SupportedLocales.Contains(settings.DefaultLocale),
                "!settings.SupportedLocales.Contains(settings.DefaultLocale)");

            LocaleChanged = localeChanged;

            DefaultLocale = settings.DefaultLocale.ToCulture();

            SupportedLocales = settings
                .SupportedLocales
                .Select(tag => tag.ToCulture())
                .ToList()
                .AsReadOnly();

            _currentLocale = DefaultLocale;
        }

        [Serializable]
        public class Settings : IEditorSettings
        {
            public LanguageTag DefaultLocale = new LanguageTag("en-US");

            public LanguageTag[] SupportedLocales =
            {
                new LanguageTag("en-US")
            };
        }
    }
}