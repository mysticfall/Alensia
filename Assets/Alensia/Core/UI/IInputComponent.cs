using System;

namespace Alensia.Core.UI
{
    public interface IInputComponent<T>
    {
        T Value { get; set; }

        IObservable<T> OnValueChange { get; }
    }
}