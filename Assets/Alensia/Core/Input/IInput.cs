using System;
using UniRx;
using Zenject;

namespace Alensia.Core.Input
{
    public interface IInput : IInitializable, ITickable, IDisposable
    {
    }
}

namespace Alensia.Core.Input.Generic
{
    public interface IInput<T> : IInput
    {
        IReadOnlyReactiveProperty<T> Value { get; }
    }
}