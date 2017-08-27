using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Event
{
    public class PointerSelectionTracker<T> : EventTracker<T> where T : UIBehaviour
    {
        private IDisposable _listener;

        public PointerSelectionTracker(T component) : base(component)
        {
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            _listener = Component.OnPointerDownAsObservable()
                .Select(_ => true)
                .Merge(Component.OnPointerUpAsObservable().Select(_ => false))
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