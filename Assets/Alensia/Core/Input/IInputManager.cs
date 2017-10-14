using System;
using System.Collections.Generic;
using Alensia.Core.Input.Generic;

namespace Alensia.Core.Input
{
    public interface IInputManager
    {
        IEnumerable<IBindingKey> Keys { get; }

        T Get<T>(IBindingKey<T> key) where T : class, IInput;

        bool Contains(IBindingKey key);

        void Register<T>(IBindingKey<T> key, T input) where T : class, IInput;

        void Deregister(IBindingKey key);

        IObservable<IBindingKey> OnBindingChange { get; }
    }
}