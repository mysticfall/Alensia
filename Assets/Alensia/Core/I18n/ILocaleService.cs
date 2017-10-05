using System;
using System.Collections.Generic;
using System.Globalization;

namespace Alensia.Core.I18n
{
    public interface ILocaleService
    {
        CultureInfo Locale { get; set; }

        CultureInfo FallbackLocale { get; }

        IReadOnlyList<CultureInfo> SupportedLocales { get; }

        IObservable<CultureInfo> OnLocaleChange { get; }
    }
}