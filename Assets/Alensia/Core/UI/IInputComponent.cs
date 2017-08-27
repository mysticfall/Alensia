using UniRx;

namespace Alensia.Core.UI
{
    public interface IInputComponent<T> : IInteractableComponent
    {
        T Value { get; set; }

        IObservable<T> OnValueChange { get; }
    }
}