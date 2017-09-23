using System;
using UniRx;
using UnityEngine.UI;

namespace Alensia.Core.UI.Event
{
    public class FocusTracker<T> : EventTracker<T> where T : InputField
    {
        private IDisposable _listener;

        public FocusTracker(T component) : base(component)
        {
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            _listener = Component
                .ObserveEveryValueChanged(c => c.isFocused)
                .Subscribe(ChangeState);
        }

        protected override void OnDeactivated()
        {
            _listener?.Dispose();
            _listener = null;

            base.OnDeactivated();
        }
    }
}