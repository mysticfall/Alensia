using System;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Common
{
    public interface IObserverHost
    {
        CompositeDisposable Disposables { get; }
    }

    public static class DisposableObserverExtensions
    {
        public static void Subscribe<T>(
            this IObserverHost host, UniRx.IObservable<T> observable, Action<T> action)
        {
            Assert.IsFalse(host.Disposables.IsDisposed, $"{nameof(host)} is already disposed.");

            observable.Subscribe(action).AddTo(host.Disposables);
        }
    }
}