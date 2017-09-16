using Alensia.Core.UI.Event;
using UniRx;

namespace Alensia.Core.UI
{
    public interface IInteractableComponent : IComponent, IHighlightable, IInteractable
    {
        string Cursor { get; }

        IObservable<string> OnCursorChange { get; }
    }
}