using System;
using UniRx;
using Zenject;

namespace Alensia.Core.Common
{
    public interface IBaseObject : IInitializable, IDisposable
    {
        bool Initialized { get; }

        bool Disposed { get; }

        UniRx.IObservable<Unit> OnInitialize { get; }

        UniRx.IObservable<Unit> OnDispose { get; }
    }
}