using System;
using System.Collections.Generic;
using System.Globalization;

namespace Alensia.Core.I18n
{
    public interface ILocaleService
    {
        IReadOnlyList<CultureInfo> SupportedLocales { get; }

        CultureInfo DefaultLocale { get; }

        CultureInfo CurrentLocale { get; set; }

        IObservable<CultureInfo> OnLocaleChange { get; }
    }
}