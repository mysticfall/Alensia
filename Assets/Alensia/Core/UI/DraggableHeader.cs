using System;
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

        public bool Interacting => _tracker != null && _tracker.Interacting;

        public bool Highlighted => _tracker != null && _tracker.Highlighted;

        public string Cursor => _cursor.Value;

        public IObservable<string> OnCursorChange =>
            _cursor.Merge(OnInteractableStateChange.Select(_ => Cursor)).DistinctUntilChanged();

        public IObservable<bool> OnInteractableStateChange => _interactable;

        public IObservable<bool> OnInteractingStateChange => _tracker?.OnInteractingStateChange;

        public IObservable<bool> OnHighlightedStateChange => _tracker?.OnHighlightedStateChange;

        public IObservable<PointerEventData> OnDragBegin => this.OnBeginDragAsObservable().Where(_ => Interactable);

        public IObservable<PointerEventData> OnDrag => this.OnDragAsObservable().Where(_ => Interactable);

        public IObservable<PointerEventData> OnDragEnd => this.OnEndDragAsObservable().Where(_ => Interactable);

        [SerializeField, PredefinedLiteral(typeof(CursorNames))] private StringReactiveProperty _cursor;

        [SerializeField] private BoolReactiveProperty _interactable;

        private InteractionHandler<DraggableHeader> _tracker;

        protected override void InitializeComponent(IUIContext context, bool isPlaying)
        {
            base.InitializeComponent(context, isPlaying);

            if (!isPlaying) return;

            _tracker = new InteractionHandler<DraggableHeader>(
                this,
                new PointerPresenceTracker<DraggableHeader>(this),
                new PointerDragTracker<DraggableHeader>(this));

            _tracker.Initialize();

            _interactable
                .Subscribe(v => _tracker.Interactable = v, Debug.LogError)
                .AddTo(this);
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