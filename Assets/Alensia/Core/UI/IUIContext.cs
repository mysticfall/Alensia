using System.Globalization;
using Alensia.Core.I18n;
using UniRx;
using UnityEngine;

namespace Alensia.Core.UI
{
    public interface IUIContext
    {
        UIStyle Style { get; }

        CultureInfo Locale { get; }

        ITranslator Translator { get; }

        IComponent ActiveComponent { get; set; }

        IObservable<UIStyle> OnStyleChange { get; }

        IObservable<CultureInfo> OnLocaleChange { get; }

        IObservable<IComponent> OnActiveComponentChange { get; }

        TUI Instantiate<TDef, TUI>(TDef definition, Transform parent)
            where TDef : IUIDefinition
            where TUI : IUIElement;

        TUI Instantiate<TUI>(GameObject item, Transform parent)
            where TUI : IUIElement;
    }
}