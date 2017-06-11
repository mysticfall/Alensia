using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Zenject;

namespace Alensia.Core.I18n
{
    public abstract class Translator : ITranslator, IInitializable, IDisposable
    {
        public ILocaleService LocaleService { get; }

        public IMessages Messages { get; private set; }

        protected Translator(ILocaleService localeService)
        {
            Assert.IsNotNull(localeService, "localeService != null");

            LocaleService = localeService;
        }

        public virtual void Initialize()
        {
            Messages = Load(LocaleService.CurrentLocale);

            LocaleService.LocaleChanged.Listen(OnLocaleChange);
        }

        public virtual void Dispose()
        {
            LocaleService.LocaleChanged.Unlisten(OnLocaleChange);

            Messages = null;
        }

        protected virtual void OnLocaleChange(CultureInfo locale) => Messages = Load(locale);

        public bool Contains(string key) => Messages?.Contains(key) ?? false;

        public virtual string this[string key] => Messages?[key];

        public virtual string Translate(string key, params object[] args)
        {
            var message = Messages?[key];

            return message == null ? key : string.Format(message, args);
        }

        protected abstract IMessages Load(CultureInfo locale, IMessages parent);

        protected IMessages Load(CultureInfo locale)
        {
            Assert.IsNotNull(locale, "locale != null");

            return GetFallbackLocaleHierarchy(locale)
                .Reverse()
                .Aggregate<CultureInfo, IMessages>(null, (chain, loc) => Load(loc, chain) ?? chain);
        }

        protected virtual IList<CultureInfo> GetFallbackLocaleHierarchy(CultureInfo locale)
        {
            Assert.IsNotNull(locale, "locale != null");

            var parent = locale;

            var locales = new List<CultureInfo>();

            while (!Equals(parent, CultureInfo.InvariantCulture) &&
                   !Equals(parent, LocaleService.DefaultLocale))
            {
                locales.Add(parent);
                parent = parent.Parent;
            }

            locales.Add(LocaleService.DefaultLocale);

            return locales;
        }
    }
}