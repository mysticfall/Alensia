using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Common
{
    public abstract class BaseObject : IBaseObject
    {
        public bool Initialized => _initialized.Value;

        public bool Disposed => _disposed.Value;

        public IObservable<Unit> OnInitialize => _initialized.Where(v => v).AsUnitObservable();

        public IObservable<Unit> OnDispose => _disposed.Where(v => v).AsUnitObservable();

        private readonly IReactiveProperty<bool> _initialized;

        private readonly IReactiveProperty<bool> _disposed;

        // ReSharper disable once CollectionNeverQueried.Global
        internal readonly ICollection<IDisposable> Disposables;

        protected BaseObject()
        {
            _initialized = new ReactiveProperty<bool>();
            _disposed = new ReactiveProperty<bool>();

            Disposables = new CompositeDisposable();
        }

        public void Initialize()
        {
            lock (this)
            {
                if (_initialized.Value)
                {
                    throw new InvalidOperationException(
                        "The object has already been initialized.");
                }

                _initialized.Value = true;
            }

            OnInitialized();
        }

        public void Dispose()
        {
            lock (this)
            {
                if (_disposed.Value)
                {
                    throw new InvalidOperationException(
                        "The object has already been disposed.");
                }

                _disposed.Value = true;
            }

            Disposables.Clear();

            OnDisposed();
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void OnDisposed()
        {
        }
    }

    public static class BaseObjectExtensions
    {
        public static void AddTo<T>(
            this T disposable, BaseObject parent) where T : IDisposable
        {
            Assert.IsNotNull(parent, "parent != null");

            parent.Disposables.Add(disposable);
        }
    }
}