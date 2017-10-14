using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.Input;
using Alensia.Core.UI.Cursor;
using UniRx;
using Zenject;

namespace Alensia.Core.Control
{
    public abstract class Control : ActivatableMonoBehavior, IControl
    {
        [Inject]
        public IInputManager InputManager { get; }

        public ICollection<IBindingKey> Bindings { get; private set; }

        public virtual CursorState CursorState => null;

        public virtual bool Valid => true;

        private bool _active;

        private readonly ICollection<IDisposable> _disposables = new CompositeDisposable();

        protected Control()
        {
            Bindings = Enumerable.Empty<IBindingKey>().ToList();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            InputManager.OnBindingChange
                .Subscribe(ProcessBindingChange)
                .AddTo(this);

            Bindings = PrepareBindings().ToList().AsReadOnly();

            RegisterDefaultBindings();

            Activate();
        }

        protected override void OnDisposed()
        {
            if (Active) Deactivate();

            Bindings = Enumerable.Empty<IBindingKey>().ToList();

            base.OnDisposed();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            Subscribe(_disposables);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            _disposables.Clear();
        }

        protected abstract void Subscribe(ICollection<IDisposable> disposables);

        protected abstract IEnumerable<IBindingKey> PrepareBindings();

        protected virtual void RegisterDefaultBindings()
        {
        }

        private void ProcessBindingChange(IBindingKey key)
        {
            if (!Bindings.Contains(key)) return;

            lock (this)
            {
                var wasActive = Active;

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