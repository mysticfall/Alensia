using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.Input;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Control
{
    public abstract class Control : BaseActivatable, IControl
    {
        public abstract string Name { get; }

        public IInputManager InputManager { get; }

        public ICollection<IBindingKey> Bindings { get; private set; }

        public virtual bool Valid => true;

        private bool _active;

        private readonly ICollection<IDisposable> _disposables = new CompositeDisposable();

        protected Control(IInputManager inputManager)
        {
            Assert.IsNotNull(inputManager, "inputManager != null");

            InputManager = inputManager;
            Bindings = Enumerable.Empty<IBindingKey>().ToList();

            OnInitialize.Subscribe(_ => AfterInitialize()).AddTo(this);
            OnDispose.Subscribe(_ => AfterDispose()).AddTo(this);

            OnActivate.Subscribe(_ => Subscribe(_disposables)).AddTo(this);
            OnDeactivate.Subscribe(_ => _disposables.Clear()).AddTo(this);

            InputManager.OnBindingChange.Subscribe(ProcessBindingChange).AddTo(this);
        }

        private void AfterInitialize()
        {
            Bindings = PrepareBindings().ToList().AsReadOnly();

            RegisterDefaultBindings();
        }

        private void AfterDispose()
        {
            if (Active.Value) Deactivate();

            Bindings = Enumerable.Empty<IBindingKey>().ToList();
        }

        protected abstract void Subscribe(ICollection<IDisposable> disposables);

        protected abstract ICollection<IBindingKey> PrepareBindings();

        protected virtual void RegisterDefaultBindings()
        {
        }

        private void ProcessBindingChange(IBindingKey key)
        {
            if (!Bindings.Contains(key)) return;

            lock (this)
            {
                var wasActive = Active.Value;

                Deactivate();

                OnBindingChange(key);

                if (Valid && wasActive) Activate();
            }
        }

        protected virtual void OnBindingChange(IBindingKey key)
        {
        }
    }
}