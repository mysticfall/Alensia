using UniRx;

namespace Alensia.Core.UI
{
    public interface IClickable<T>
    {
        IObservable<T> Clicked { get; }
    }
}