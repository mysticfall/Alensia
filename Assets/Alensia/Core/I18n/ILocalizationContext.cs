using System;
using System.Globalization;

namespace Alensia.Core.I18n
{
    public interface ILocalizationContext
    {
        CultureInfo Locale { get; }

        ITranslator Translator { get; }

        IObservable<CultureInfo> OnLocaleChange { get; }
    }
}