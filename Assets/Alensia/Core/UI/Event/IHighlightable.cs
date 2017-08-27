using UniRx;

namespace Alensia.Core.UI.Event
{
    public interface IHighlightable
    {
        bool Highlighted { get; }

        IObservable<bool> OnHighlightedStateChange { get; }
    }
}