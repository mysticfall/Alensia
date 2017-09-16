using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.UI.Cursor;
using Alensia.Core.UI.Event;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.String;

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

        public string Cursor => IsNullOrWhiteSpace(_cursor.Value) ? FirstActiveAncestor?.Cursor : _cursor.Value;

        public IObservable<string> OnCursorChange => _cursor;

        public IObservable<bool> OnInteractableStateChange => _interactable;

        public IObservable<bool> OnInteractingStateChange => _interactionTracker?.OnStateChange;

        public IObservable<bool> OnHighlightedStateChange => _highlightTracker?.OnStateChange;

        protected abstract TSelectable PeerSelectable { get; }

        protected abstract THotspot PeerHotspot { get; }

        [SerializeField] private BoolReactiveProperty _interactable;

        [SerializeField, PredefinedLiteral(typeof(CursorNames))] private StringReactiveProperty _cursor;

        private EventTracker<THotspot> _interactionTracker;

        private EventTracker<THotspot> _highlightTracker;

        private List<EventTracker<THotspot>> _trackers;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            PeerSelectable.transition = Selectable.Transition.None;

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

            var onStateChange = OnHighlightedStateChange
                .Merge(OnInteractableStateChange)
                .Merge(OnInteractingStateChange);

            onStateChange
                .Select(_ => Style)
                .Where(v => v != null)
                .Subscribe(_ => OnStyleChanged(Style))
                .AddTo(this);

            onStateChange
                .Where(_ => (Interacting || Highlighted) && !HasActiveChild())
                .Where(_ => Context.ActiveComponent == null || !Context.ActiveComponent.Interacting)
                .Subscribe(_ => Context.ActiveComponent = this)
                .AddTo(this);

            onStateChange
                .AsUnitObservable()
                .Merge(OnHide)
                .Where(_ => ReferenceEquals(Context.ActiveComponent, this))
                .Where(_ => !Interacting && !Highlighted)
                .Subscribe(_ => Context.ActiveComponent = FirstActiveAncestor)
                .AddTo(this);
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            PeerSelectable.interactable = _interactable.Value;
            PeerSelectable.transition = Selectable.Transition.None;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _trackers?.ForEach(t => t.Dispose());
            _trackers = null;

            if (Context != null && ReferenceEquals(Context.ActiveComponent, this))
            {
                Context.ActiveComponent = FirstActiveAncestor;
            }
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (IInteractableComponent) component;

            Interactable = source.Interactable;
            _cursor.Value = source.Cursor;
        }

        protected virtual EventTracker<THotspot> CreateInterationTracker() =>
            new PointerSelectionTracker<THotspot>(PeerHotspot);

        protected virtual EventTracker<THotspot> CreateHighlightTracker() =>
            new PointerPresenceTracker<THotspot>(PeerHotspot);

        private bool HasActiveChild()
        {
            var active = Context.ActiveComponent;

            return active?.Ancestors.FirstOrDefault(c => ReferenceEquals(c, this)) != null;
        }

        private IInteractableComponent FirstActiveAncestor =>
            Ancestors.FirstOrDefault(a => a is IInteractableComponent && a.Visible) as IInteractableComponent;
    }
}