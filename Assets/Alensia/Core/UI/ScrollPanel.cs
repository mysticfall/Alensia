using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.UI.Property;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Alensia.Core.UI
{
    public class ScrollPanel : Panel
    {
        public float ScrollSensitivity
        {
            get { return _scrollSensitivity.Value; }
            set { _scrollSensitivity.Value = value; }
        }

        public ScrollBar HorizontalScrollBar
        {
            get
            {
                lock (this)
                {
                    if (_horizontalScrollBar == null)
                    {
                        var peer = Transform.Find("Scrollbar Horizontal")?.gameObject;

                        _horizontalScrollBar = peer?.GetComponent<ScrollBar>();
                    }
                }

                return _horizontalScrollBar;
            }
        }

        public ScrollBar VerticalScrollBar
        {
            get
            {
                lock (this)
                {
                    if (_verticalScrollBar == null)
                    {
                        var peer = Transform.Find("Scrollbar Vertical")?.gameObject;

                        _verticalScrollBar = peer?.GetComponent<ScrollBar>();
                    }
                }

                return _verticalScrollBar;
            }
        }

        public ScrollbarVisibility HorizontalScrollbarVisibility
        {
            get { return _horizontalScrollbarVisibility.Value; }
            set { _horizontalScrollbarVisibility.Value = value; }
        }

        public ScrollbarVisibility VerticalScrollbarVisibility
        {
            get { return _verticalScrollbarVisibility.Value; }
            set { _verticalScrollbarVisibility.Value = value; }
        }

        public override IList<IComponent> Children => Content.Cast<Transform>()
            .Select(c => c.GetComponent<IComponent>())
            .Where(c => c != null)
            .ToList();

        protected ScrollRect PeerScrollRect => _peerScrollRect ?? (_peerScrollRect = GetComponent<ScrollRect>());

        protected Mask PeerMask => _peerMask ?? (_peerMask = Content.GetComponent<Mask>());

        protected Image PeerMaskImage => _peerMaskImage ?? (_peerMaskImage = Content.GetComponent<Image>());

        protected Transform Content => _content ?? (_content = Transform.Find("Viewport"));

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["ScrollPanel.Background"];

                return value == null ? base.DefaultBackground : value.Merge(base.DefaultBackground);
            }
        }

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerScrollRect != null) peers.Add(PeerScrollRect);
                if (PeerMask != null) peers.Add(PeerMask);
                if (PeerMaskImage != null) peers.Add(PeerMaskImage);

                return peers;
            }
        }

        [SerializeField] private FloatReactiveProperty _scrollSensitivity;

        [SerializeField] private ScrollbarVisibilityReactiveProperty _horizontalScrollbarVisibility;

        [SerializeField] private ScrollbarVisibilityReactiveProperty _verticalScrollbarVisibility;

        [SerializeField, HideInInspector] private ScrollBar _horizontalScrollBar;

        [SerializeField, HideInInspector] private ScrollBar _verticalScrollBar;

        [SerializeField, HideInInspector] private ScrollRect _peerScrollRect;

        [SerializeField, HideInInspector] private Mask _peerMask;

        [SerializeField, HideInInspector] private Image _peerMaskImage;

        [NonSerialized] private Transform _content;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            if (!Application.isPlaying) return;

            HorizontalScrollBar?.Initialize(Context);
            VerticalScrollBar?.Initialize(Context);
        }

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            _horizontalScrollbarVisibility
                .Merge(_verticalScrollbarVisibility)
                .Subscribe(_ => UpdateScrollbarVisibility())
                .AddTo(this);

            _scrollSensitivity
                .Subscribe(v => PeerScrollRect.scrollSensitivity = v)
                .AddTo(this);
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            UpdateScrollbarVisibility();
        }

        private void UpdateScrollbarVisibility()
        {
            var horizontalVisibility = GetVisibility(HorizontalScrollbarVisibility);

            if (horizontalVisibility.HasValue)
            {
                PeerScrollRect.horizontal = true;
                PeerScrollRect.horizontalScrollbarVisibility = horizontalVisibility.Value;
            }
            else
            {
                PeerScrollRect.horizontal = false;
            }

            var verticalVisibility = GetVisibility(VerticalScrollbarVisibility);

            if (verticalVisibility.HasValue)
            {
                PeerScrollRect.vertical = true;
                PeerScrollRect.verticalScrollbarVisibility = verticalVisibility.Value;
            }
            else
            {
                PeerScrollRect.vertical = false;
            }
        }

        private ScrollRect.ScrollbarVisibility? GetVisibility(ScrollbarVisibility value)
        {
            switch (value)
            {
                case ScrollbarVisibility.Hidden:
                    return null;
                case ScrollbarVisibility.AutoHide:
                    return ScrollRect.ScrollbarVisibility.AutoHide;
                case ScrollbarVisibility.AutoHideAndExpand:
                    return ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
                case ScrollbarVisibility.Visible:
                    return ScrollRect.ScrollbarVisibility.Permanent;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (ScrollPanel) component;

            ScrollSensitivity = source.ScrollSensitivity;

            HorizontalScrollbarVisibility = source.HorizontalScrollbarVisibility;
            VerticalScrollbarVisibility = source.VerticalScrollbarVisibility;
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public new static ScrollPanel CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/ScrollPanel");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<ScrollPanel>();
        }
    }

    public enum ScrollbarVisibility
    {
        Hidden,
        AutoHide,
        AutoHideAndExpand,
        Visible
    }

    [Serializable]
    internal class ScrollbarVisibilityReactiveProperty : ReactiveProperty<ScrollbarVisibility>
    {
    }
}