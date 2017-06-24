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

        public IReactiveProperty<CultureInfo> CurrentLocale { get;  }

        public LocaleService() : this(new Settings())
        {
        }

        [Inject]
        public LocaleService(Settings settings)
        {
            Assert.IsNotNull(settings, "settings != null");

            Assert.IsNotNull(settings.SupportedLocales, "settings.SupportedLocales != null");

            Assert.IsTrue(
                settings.SupportedLocales.Contains(settings.DefaultLocale),
                "!settings.SupportedLocales.Contains(settings.DefaultLocale)");

            DefaultLocale = settings.DefaultLocale.ToCulture();
            CurrentLocale = new ReactiveProperty<CultureInfo>(DefaultLocale);

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