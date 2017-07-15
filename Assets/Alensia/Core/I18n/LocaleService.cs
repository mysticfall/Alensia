using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.Common;
using UniRx;
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
            get { return _locale.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _locale.Value = value;
            }
        }

        public UniRx.IObservable<CultureInfo> OnLocaleChange => _locale;

        private readonly IReactiveProperty<CultureInfo> _locale;

        public LocaleService() : this(null)
        {
        }

        [Inject]
        public LocaleService([InjectOptional] Settings settings)
        {
            settings = settings ?? new Settings();

            Assert.IsNotNull(settings.SupportedLocales, "settings.SupportedLocales != null");

            Assert.IsTrue(
                settings.SupportedLocales.Contains(settings.DefaultLocale),
                "!settings.SupportedLocales.Contains(settings.DefaultLocale)");

            DefaultLocale = settings.DefaultLocale.ToCulture();

            _locale = new ReactiveProperty<CultureInfo>(DefaultLocale);

            SupportedLocales = settings
                .SupportedLocales
                .Select(tag => tag.ToCulture())
                .ToList()
                .AsReadOnly();
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