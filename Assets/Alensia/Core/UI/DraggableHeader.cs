using Alensia.Core.Common;
using Alensia.Core.UI.Cursor;
using Alensia.Core.UI.Event;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI
{
    public class DraggableHeader : Header, IInteractableComponent, IPointerDragAware
    {
        public bool Interactable
        {
            get { return _interactable.Value; }
            set { _interactable.Value = value; }
        }

        public bool Interacting => _interactionTracker != null && _interactionTracker.State;

        public bool Highlighted => _highlightTracker != null && _highlightTracker.State;

        public string Cursor => _cursor.Value;

        public IObservable<string> OnCursorChange => _cursor;

        public IObservable<bool> OnInteractableStateChange => _interactable;

        public IObservable<bool> OnInteractingStateChange => _interactionTracker?.OnStateChange;

        public IObservable<bool> OnHighlightedStateChange => _highlightTracker?.OnStateChange;

        public IObservable<PointerEventData> OnDragBegin => this.OnBeginDragAsObservable().Where(_ => Interactable);

        public IObservable<PointerEventData> OnDrag => this.OnDragAsObservable().Where(_ => Interactable);

        public IObservable<PointerEventData> OnDragEnd => this.OnEndDragAsObservable().Where(_ => Interactable);

        [SerializeField, PredefinedLiteral(typeof(CursorNames))] private StringReactiveProperty _cursor;

        [SerializeField] private BoolReactiveProperty _interactable;

        private PointerPresenceTracker<DraggableHeader> _highlightTracker;

        private PointerDragTracker<DraggableHeader> _interactionTracker;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            _highlightTracker = new PointerPresenceTracker<DraggableHeader>(this);
            _highlightTracker.Initialize();

            _interactionTracker = new PointerDragTracker<DraggableHeader>(this);
            _interactionTracker.Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _highlightTracker?.Dispose();
            _highlightTracker = null;

            _interactionTracker?.Dispose();
            _interactionTracker = null;
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (DraggableHeader) component;

            _cursor.Value = source.Cursor;
            Interactable = source.Interactable;

        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public new static DraggableHeader CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/DraggableHeader");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<DraggableHeader>();
        }
    }
}