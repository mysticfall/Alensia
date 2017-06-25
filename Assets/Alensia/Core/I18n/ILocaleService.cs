using System.Collections.Generic;
using System.Globalization;
using UniRx;

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