using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.Common;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.I18n
{
    public abstract class Translator : BaseObject, ITranslator
    {
        public ILocaleService LocaleService { get; }

        public IMessages Messages { get; private set; }

        protected Translator(ILocaleService localeService)
        {
            Assert.IsNotNull(localeService, "localeService != null");

            LocaleService = localeService;

            LocaleService.CurrentLocale
                .Where(_ => Initialized)
                .Subscribe(OnLocaleChange)
                .AddTo(this);

            OnInitialize
                .Subscribe(_ => Messages = Load(LocaleService.CurrentLocale.Value))
                .AddTo(this);
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