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
        public abstract string Name { get; }

        public IInputManager InputManager { get; }

        public ICollection<IBindingKey> Bindings { get; private set; }

        protected readonly CompositeDisposable Observers;

        protected readonly CompositeDisposable ConstantObservers;

        public bool Active
        {
            get { return _active; }
            set
            {
                lock (this)
                {
                    if (_active == value) return;

                    _active = value;

                    Observers.Clear();

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
        }

        public virtual bool Valid => true;

        private bool _active;

        protected Control(IInputManager inputManager)
        {
            Assert.IsNotNull(inputManager, "inputManager != null");

            InputManager = inputManager;
            Bindings = Enumerable.Empty<IBindingKey>().ToList();

            Observers = new CompositeDisposable();
            ConstantObservers = new CompositeDisposable();
        }

        public virtual void Initialize()
        {
            Bindings = PrepareBindings().ToList().AsReadOnly();

            InputManager.BindingChanged.Listen(ProcessBindingChange);

            RegisterDefaultBindings();
        }

        public virtual void Dispose()
        {
            if (Active) Active = false;

            Bindings = Enumerable.Empty<IBindingKey>().ToList();

            InputManager.BindingChanged.Unlisten(ProcessBindingChange);

            ConstantObservers.Clear();
        }

        protected virtual void OnActivate()
        {
        }

        protected virtual void OnDeactivate()
        {
        }

        protected abstract ICollection<IBindingKey> PrepareBindings();

        protected virtual void RegisterDefaultBindings()
        {
        }

        private void ProcessBindingChange(IBindingKey key)
        {
            if (!Bindings.Contains(key)) return;

            lock (this)
            {
                var active = Active;

                Active = false;

                OnBindingChange(key);

                Active = Valid && active;
            }
        }

        protected virtual void OnBindingChange(IBindingKey key)
        {
        }
    }
}