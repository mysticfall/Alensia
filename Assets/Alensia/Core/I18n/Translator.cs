using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.Common;
using UniRx;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.I18n
{
    public abstract class Translator : ManagedMonoBehavior, ITranslator
    {
        [Inject]
        public ILocaleService LocaleService { get; }

        public IMessages Messages { get; private set; }

        protected override void OnInitialized()
        {
            LocaleService.OnLocaleChange
                .Where(_ => Initialized)
                .Subscribe(OnLocaleChange)
                .AddTo(this);

            Messages = Load(LocaleService.Locale);

            base.OnInitialized();
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

        protected virtual IEnumerable<CultureInfo> GetFallbackLocaleHierarchy(CultureInfo locale)
        {
            Assert.IsNotNull(locale, "locale != null");

            var parent = locale;

            var locales = new List<CultureInfo>();

            while (!Equals(parent, CultureInfo.InvariantCulture) &&
                   !Equals(parent, LocaleService.FallbackLocale))
            {
                locales.Add(parent);
                parent = parent.Parent;
            }

            locales.Add(LocaleService.FallbackLocale);

            return locales;
        }
    }
}