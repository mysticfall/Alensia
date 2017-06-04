using System.Globalization;
using Zenject;

namespace Alensia.Core.I18n
{
    public class LocaleChangeEvent : Signal<CultureInfo, LocaleChangeEvent>
    {
    }
}