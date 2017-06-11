using Alensia.Core.UI.Legacy;
using UnityEngine;

namespace Alensia.Core.UI
{
    public interface IUIManager : IComponentsHolder
    {
        GUISkin Skin { get; set; }

        CursorDefinition ActiveCursor { get; set; }

        void ShowCursor();

        void HideCursor();
    }
}