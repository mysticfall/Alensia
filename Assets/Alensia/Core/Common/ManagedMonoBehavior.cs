using System;
using Alensia.Core.Geom;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Common
{
    public abstract class ManagedMonoBehavior : MonoBehaviour, IManagedObject, ITransformable
    {
        public bool Initialized => _initialized.Value;

        public bool Disposed => _disposed.Value;

        public GameObject GameObject => gameObject;

        public Transform Transform => transform;

        public IObservable<Unit> OnInitialize => _initialized.Where(v => v).AsUnitObservable();

        public IObservable<Unit> OnDispose => _disposed.Where(v => v).AsUnitObservable();

        private readonly IReactiveProperty<bool> _initialized;

        private readonly IReactiveProperty<bool> _disposed;

        protected ManagedMonoBehavior()
        {
            _initialized = new ReactiveProperty<bool>();
            _disposed = new ReactiveProperty<bool>();
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

            OnDisposed();
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void OnDisposed()
        {
        }
    }
}