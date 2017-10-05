using System;
using Alensia.Core.Common;
using UniRx;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Event
{
    public abstract class EventTracker<T> : ActivatableObject
        where T : UIBehaviour
    {
        public T Component { get; }

        public bool State => _state.Value;

        public IObservable<bool> OnStateChange => _state;

        private readonly IReactiveProperty<bool> _state;

        protected EventTracker(T component)
        {
            Assert.IsNotNull(component, "component != null");

            _state = new ReactiveProperty<bool>();

            Component = component;
        }

        protected void ChangeState(bool state) => _state.Value = state;
    }
}