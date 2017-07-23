using UniRx;

namespace Alensia.Core.UI
{
    public interface IInputComponent<T> : IComponent
    {
        T Value { get; set; }

        IObservable<T> OnValueChange { get; }
    }
}