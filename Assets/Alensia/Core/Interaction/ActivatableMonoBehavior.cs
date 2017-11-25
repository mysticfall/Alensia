using System;
using Alensia.Core.Common;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Interaction
{
    public class ActivatableMonoBehavior : ManagedMonoBehavior, IActivatable
    {
        public bool Active
        {
            get { return _active.Value; }
            set
            {
                if (Initialized)
                {
                    if (Disposed && value)
                    {
                        throw new InvalidOperationException(
                            "Disposed object cannot be activated.");
                    }

                    _active.Value = value;
                }
                else
                {
                    _lazyActivation = value;
                }
            }
        }

        public IObservable<Unit> OnActivate => _active.Where(v => v).AsUnitObservable();

        public IObservable<Unit> OnDeactivate => _active.Where(v => !v).AsUnitObservable();

        public IObservable<bool> OnActiveStateChange => _active;

        private bool _lazyActivation;

        private readonly IReactiveProperty<bool> _active;

        protected ActivatableMonoBehavior()
        {
            _active = new ReactiveProperty<bool>();
        }

        public void Activate() => Active = true;

        public void Deactivate() => Active = false;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            OnActivate
                .Subscribe(_ => OnActivated(), Debug.LogError)
                .AddTo(this);
            OnDeactivate
                .Subscribe(_ => OnDeactivated(), Debug.LogError)
                .AddTo(this);

            if (_lazyActivation) Activate();
        }

        protected virtual void OnActivated()
        {
        }

        protected virtual void OnDeactivated()
        {
        }
    }
}