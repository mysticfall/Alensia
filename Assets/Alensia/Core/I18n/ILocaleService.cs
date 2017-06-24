using System.Collections.Generic;
using System.Globalization;
using UniRx;

namespace Alensia.Core.I18n
{
    public interface ILocaleService
    {
        IReadOnlyList<CultureInfo> SupportedLocales { get; }

        CultureInfo DefaultLocale { get; }

        IReactiveProperty<CultureInfo> CurrentLocale { get; }
    }
}