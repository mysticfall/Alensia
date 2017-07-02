using Alensia.Core.I18n;
using UnityEngine;
using Zenject;

namespace Alensia.Core.UI
{
    public interface IUIContext
    {
        DiContainer DiContainer { get; }

        ITranslator Translator { get; }

        IComponent ActiveComponent { get; set; }

        TUI Instantiate<TDef, TUI>(TDef definition, Transform parent)
            where TDef : UIDefinition
            where TUI : IUIElement;
    }
}