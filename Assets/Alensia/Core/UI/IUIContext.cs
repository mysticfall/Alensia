using System;
using Alensia.Core.Common;
using Alensia.Core.I18n;
using Alensia.Core.UI.Cursor;
using Alensia.Core.UI.Screen;
using UnityEngine;

namespace Alensia.Core.UI
{
    public interface IUIContext : IScreenManager, IStylable, ILocalizationContext, IInjectionRoot
    {
        CursorState CursorState { get; set; }

        string DefaultCursor { get; set; }

        IInteractableComponent ActiveComponent { get; set; }

        IObservable<IInteractableComponent> OnActiveComponentChange { get; }

        TUI Instantiate<TUI>(GameObject prefab, Transform parent) where TUI : IUIElement;
    }
}