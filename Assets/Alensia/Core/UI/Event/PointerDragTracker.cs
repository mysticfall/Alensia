using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Event
{
    public class PointerDragTracker<T> : EventTracker<T> where T : UIBehaviour
    {
        private IDisposable _listener;

        public PointerDragTracker(T component) : base(component)
        {
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            _listener = Component.OnBeginDragAsObservable()
                .Select(_ => true)
                .Merge(Component.OnEndDragAsObservable().Select(_ => false))
                .DistinctUntilChanged()
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