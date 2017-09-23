using System.Collections.Generic;
using Alensia.Core.Common;
using Alensia.Core.UI.Property;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    public class ScrollBar : InteractableComponent<Scrollbar, Scrollbar>
    {
        public ImageAndColorSet Background
        {
            get { return _background.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _background.Value = value;
            }
        }

        public ImageAndColorSet HandleImage
        {
            get { return _handleImage.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _handleImage.Value = value;
            }
        }

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = DefaultBackgroundSet;

                return value?.ValueFor(this)?.Merge(base.DefaultBackground) ?? base.DefaultBackground;
            }
        }

        protected virtual ImageAndColorSet DefaultBackgroundSet =>
            Style?.ImageAndColorSets?["ScrollPanel.SlidingArea"];

        protected virtual ImageAndColor DefaultHandleImage => DefaultHandleImageSet?.ValueFor(this);

        protected virtual ImageAndColorSet DefaultHandleImageSet =>
            Style?.ImageAndColorSets?["ScrollPanel.HandleImage"];

        protected Scrollbar Peer => _peer ?? (_peer = GetComponent<Scrollbar>());

        protected Transform PeerSlideArea => _peerSlideArea ?? (_peerSlideArea = Transform.Find("Sliding Area"));

        protected Image PeerBackground => _peerBackground ?? (_peerBackground = GetComponent<Image>());

        protected Image PeerHandle
        {
            get
            {
                if (_peerHandle != null) return _peerHandle;

                _peerHandle = PeerSlideArea.FindComponent<Image>("Handle");

                return _peerHandle;
            }
        }

        protected override Scrollbar PeerSelectable => Peer;

        protected override Scrollbar PeerHotspot => Peer;

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (Peer != null) peers.Add(Peer);
                if (PeerBackground != null) peers.Add(PeerBackground);
                if (PeerSlideArea != null) peers.Add(PeerSlideArea);

                return peers;
            }
        }

        [SerializeField] private ImageAndColorSetReactiveProperty _handleImage;

        [SerializeField] private ImageAndColorSetReactiveProperty _background;

        [SerializeField, HideInInspector] private Scrollbar _peer;

        [SerializeField, HideInInspector] private Image _peerBackground;

        [SerializeField, HideInInspector] private Image _peerHandle;

        private Transform _peerSlideArea;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            _background
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerBackground, DefaultBackground))
                .AddTo(this);
            _handleImage
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerHandle, DefaultHandleImage))
                .AddTo(this);
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            Peer.size = 0.2f;
            Peer.value = 0;
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            Background.ValueFor(this).Update(PeerBackground, DefaultBackground);
            HandleImage.ValueFor(this).Update(PeerHandle, DefaultHandleImage);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (ScrollBar) component;

            Background = new ImageAndColorSet(source.Background);
            HandleImage = new ImageAndColorSet(source.HandleImage);
        }

        protected override UIComponent CreatePristineInstance()
        {
            var prefab = ScrollPanel.CreateInstance();
            var instance = prefab.Transform.Find(Name);

            instance.SetParent(Transform);

            DestroyImmediate(prefab.gameObject);

            return instance.GetComponent<ScrollBar>();
        }
    }
}