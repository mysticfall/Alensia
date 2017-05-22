using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Input;
using UniRx;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Control
{
    public abstract class Control : IControl, IInitializable, IDisposable
    {
        public IInputManager InputManager { get; }

        public ICollection<IBindingKey> Bindings { get; private set; }

        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active == value) return;

                _active = value;

                _disposables.Clear();

                if (_active)
                {
                    OnActivate();
                }
                else
                {
                    OnDeactivate();
                }
            }
        }

        public virtual bool Valid => true;

        private bool _active;

        private readonly CompositeDisposable _disposables;

        protected Control(IInputManager inputManager)
        {
            Assert.IsNotNull(inputManager, "inputManager != null");

            InputManager = inputManager;
            Bindings = Enumerable.Empty<IBindingKey>().ToList();

            _disposables = new CompositeDisposable();
        }

        public virtual void Initialize()
        {
            Bindings = PrepareBindings().ToList().AsReadOnly();

            InputManager.BindingChanged.Listen(ProcessBindingChange);
        }

        public virtual void Dispose()
        {
            if (Active) Active = false;

            Bindings = Enumerable.Empty<IBindingKey>().ToList();

            InputManager.BindingChanged.Unlisten(ProcessBindingChange);
        }

        protected virtual void OnActivate()
        {
        }

        protected virtual void OnDeactivate()
        {
        }

        protected abstract ICollection<IBindingKey> PrepareBindings();

        private void ProcessBindingChange(IBindingKey key)
        {
            if (!Bindings.Contains(key)) return;

            var active = Active;

            Active = false;

            OnBindingChange(key);

            Active = active;
        }

        protected virtual void OnBindingChange(IBindingKey key)
        {
        }

        protected void Subsribe<T>(UniRx.IObservable<T> observable, Action<T> action)
        {
            observable.Where(_ => Valid && Active).Subscribe(action).AddTo(_disposables);
        }
    }
}