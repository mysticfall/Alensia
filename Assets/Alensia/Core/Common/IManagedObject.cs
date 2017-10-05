using System;
using UniRx;
using Zenject;

namespace Alensia.Core.Common
{
    public interface IManagedObject : IInitializable, IDisposable
    {
        bool Initialized { get; }

        bool Disposed { get; }

        IObservable<Unit> OnInitialize { get; }

        IObservable<Unit> OnDispose { get; }
    }
}