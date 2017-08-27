using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Event
{
    public class PointerPresenceTracker<T> : EventTracker<T> where T : UIBehaviour
    {
        private IDisposable _listener;

        public PointerPresenceTracker(T component) : base(component)
        {
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            _listener = Component.OnPointerEnterAsObservable()
                .Select(_ => true)
                .Merge(Component.OnPointerExitAsObservable().Select(_ => false))
                .DistinctUntilChanged()
                .Subscribe(v => Active = v);
        }

        protected override void OnDeactivated()
        {
            _listener?.Dispose();
            _listener = null;

            base.OnDeactivated();
        }
    }
}