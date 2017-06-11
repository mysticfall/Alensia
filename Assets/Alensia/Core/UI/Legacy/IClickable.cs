using UniRx;

namespace Alensia.Core.UI.Legacy
{
    public interface IClickable<T>
    {
        IObservable<T> Clicked { get; }
    }
}