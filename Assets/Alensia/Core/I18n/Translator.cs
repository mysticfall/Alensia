using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;

namespace Alensia.Core.I18n
{
    public abstract class Translator : ITranslator
    {
        public ILocaleService LocaleService { get; }

        public ITranslationSet TranslationSet
        {
            get
            {
                lock (this)
                {
                    var locale = LocaleService.CurrentLocale;

                    ITranslationSet set;

                    if (_translations.TryGetValue(locale.Name, out set)) return set;

                    set = Load(locale);

                    _translations.Add(locale.Name, set);

                    return set;
                }
            }
        }

        private readonly IDictionary<string, ITranslationSet> _translations;

        protected Translator(ILocaleService localeService)
        {
            Assert.IsNotNull(localeService, "localeService != null");

            LocaleService = localeService;

            _translations = new Dictionary<string, ITranslationSet>();
        }

        public virtual string this[string key] => TranslationSet?[key];

        public virtual string Translate(string key, params object[] args)
        {
            var message = TranslationSet?[key];

            return message == null ? key : string.Format(message, args);
        }

        protected abstract ITranslationSet Load(CultureInfo locale);
    }
}