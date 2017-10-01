using System;
using Alensia.Core.Common;
using Alensia.Core.UI.Cursor;
using Alensia.Core.UI.Event;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.RectTransform.Axis;

namespace Alensia.Core.UI.Resize
{
    public abstract class ResizeHandle : UIComponent, IInteractableComponent, IPointerDragAware
    {
        public IComponent Target;

        public float Size
        {
            get { return _size.Value; }
            set { _size.Value = value; }
        }

        public bool Interactable
        {
            get { return _interactable.Value; }
            set { _interactable.Value = value; }
        }

        public bool Interacting => _tracker != null && _tracker.Interacting;

        public bool Highlighted => _tracker != null && _tracker.Highlighted;

        public abstract string Cursor { get; }

        public IObservable<string> OnCursorChange => _cursor;

        public IObservable<bool> OnInteractableStateChange => _interactable;

        public IObservable<bool> OnInteractingStateChange => _tracker?.OnInteractingStateChange;

        public IObservable<bool> OnHighlightedStateChange => _tracker?.OnHighlightedStateChange;

        public IObservable<PointerEventData> OnDragBegin => this.OnBeginDragAsObservable().Where(_ => Interactable);

        public IObservable<PointerEventData> OnDrag => this.OnDragAsObservable().Where(_ => Interactable);

        public IObservable<PointerEventData> OnDragEnd => this.OnEndDragAsObservable().Where(_ => Interactable);

        [SerializeField] private FloatReactiveProperty _size;

        [SerializeField, PredefinedLiteral(typeof(CursorNames))] private StringReactiveProperty _cursor;

        [SerializeField] private BoolReactiveProperty _interactable;

        [SerializeField, HideInInspector] private Image _peerImage;

        [SerializeField, HideInInspector] private LayoutElement _layout;

        private InteractionHandler<ResizeHandle> _tracker;

        protected ResizeHandle()
        {
            _size = new FloatReactiveProperty(10);
            _cursor = new StringReactiveProperty();
            _interactable = new BoolReactiveProperty(true);
        }

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            var layout = this.GetOrAddComponent<LayoutElement>();

            layout.ignoreLayout = true;

            var image = this.GetOrAddComponent<Image>();

            image.color = Color.clear;

            UpdatePosition(RectTransform);

            _cursor.Value = Cursor;

            _tracker = new InteractionHandler<ResizeHandle>(
                this,
                new PointerPresenceTracker<ResizeHandle>(this),
                new PointerDragTracker<ResizeHandle>(this));

            _tracker.Initialize();

            _interactable.Subscribe(v => _tracker.Interactable = v).AddTo(this);

            OnDrag
                .Where(_ => Target != null)
                .Subscribe(Resize)
                .AddTo(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _tracker?.Dispose();
            _tracker = null;
        }

        private void Resize(PointerEventData e)
        {
            var t = Target.RectTransform;

            var size = CalculateSizeDelta(e);
            var origin = CalculateAnchor(t.rect);

            t.SetSizeWithCurrentAnchors(Horizontal, t.rect.width + size.x);
            t.SetSizeWithCurrentAnchors(Vertical, t.rect.height + size.y);

            var pos = origin - CalculateAnchor(t.rect);
            var anchor = t.anchoredPosition;

            anchor.Set(anchor.x + pos.x, anchor.y + pos.y);

            t.anchoredPosition = anchor;
        }

        protected abstract Vector2 CalculateAnchor(Rect rect);

        protected abstract Vector2 CalculateSizeDelta(PointerEventData e);

        protected abstract void UpdatePosition(RectTransform rectTransform);


        protected override UIComponent CreatePristineInstance()
        {
            throw new NotImplementedException();
        }
    }
}