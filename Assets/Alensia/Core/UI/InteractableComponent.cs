using System.Collections.Generic;
using Alensia.Core.UI.Event;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    public abstract class InteractableComponent<TSelectable, THotspot> : UIComponent, IInteractableComponent
        where TSelectable : Selectable
        where THotspot : UIBehaviour
    {
        public bool Interactable
        {
            get { return _interactable.Value; }
            set { _interactable.Value = value; }
        }

        public bool Interacting => _interactionTracker != null && _interactionTracker.State;

        public bool Highlighted => _highlightTracker != null && _highlightTracker.State;

        public IObservable<bool> OnInteractableStateChange => _interactable;

        public IObservable<bool> OnInteractingStateChange => _interactionTracker?.OnStateChange;

        public IObservable<bool> OnHighlightedStateChange => _highlightTracker?.OnStateChange;

        protected abstract TSelectable PeerSelectable { get; }

        protected abstract THotspot PeerHotspot { get; }

        [SerializeField] private BoolReactiveProperty _interactable;

        private EventTracker<THotspot> _interactionTracker;

        private EventTracker<THotspot> _highlightTracker;

        private List<EventTracker<THotspot>> _trackers;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            _interactionTracker = CreateInterationTracker();
            _highlightTracker = CreateHighlightTracker();

            _trackers = new List<EventTracker<THotspot>> {_interactionTracker, _highlightTracker};

            _trackers.ForEach(t => t.Initialize());

            _interactable
                .Subscribe(v =>
                {
                    PeerSelectable.interactable = v;
                    _trackers.ForEach(t => t.Active = v);
                })
                .AddTo(this);
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            PeerSelectable.interactable = _interactable.Value;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _trackers?.ForEach(t => t.Dispose());
            _trackers = null;
        }
        
        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (IInteractableComponent) component;

            Interactable = source.Interactable;
        }

        protected virtual EventTracker<THotspot> CreateInterationTracker() =>
            new PointerSelectionTracker<THotspot>(PeerHotspot);

        protected virtual EventTracker<THotspot> CreateHighlightTracker()=>
            new PointerPresenceTracker<THotspot>(PeerHotspot);
    }
}