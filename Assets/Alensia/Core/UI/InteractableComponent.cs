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

        public bool Interacting => _tracker != null && _tracker.Interacting;

        public bool Highlighted => _tracker != null && _tracker.Highlighted;

        public string Cursor =>
            IsNullOrWhiteSpace(_cursor.Value) ? this.FindFirstActiveAncestor()?.Cursor : _cursor.Value;

        public IObservable<string> OnCursorChange => _cursor;

        public IObservable<bool> OnInteractableStateChange => _interactable;

        public IObservable<bool> OnInteractingStateChange => _tracker?.OnInteractingStateChange;

        public IObservable<bool> OnHighlightedStateChange => _tracker?.OnHighlightedStateChange;

        protected abstract TSelectable PeerSelectable { get; }

        protected abstract THotspot PeerHotspot { get; }

        [SerializeField] private BoolReactiveProperty _interactable;

        [SerializeField, PredefinedLiteral(typeof(CursorNames))] private StringReactiveProperty _cursor;

        private InteractionHandler<THotspot> _tracker;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            PeerSelectable.transition = Selectable.Transition.None;

            _tracker = new InteractionHandler<THotspot>(this, CreateHighlightTracker(), CreateInterationTracker());
            _tracker.Initialize();

            _interactable.Subscribe(v => _tracker.Interactable = v).AddTo(this);

            _tracker.OnStateChange
                .Select(_ => Style)
                .Where(v => v != null)
                .Subscribe(_ => OnStyleChanged(Style))
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

            _tracker?.Dispose();
            _tracker = null;
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
    }
}