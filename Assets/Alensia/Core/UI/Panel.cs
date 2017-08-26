using System.Collections.Generic;
using Alensia.Core.UI.Property;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    public class Panel : UIContainer
    {
        public bool Opaque
        {
            get { return _opaque.Value; }
            set { _opaque.Value = value; }
        }

        public ImageAndColor Background
        {
            get { return _background.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _background.Value = value;
            }
        }

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["Panel.Background"];

                return value == null ? base.DefaultBackground : value.Merge(base.DefaultBackground);
            }
        }

        protected Image PeerImage => _peerImage;

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerImage != null) peers.Add(PeerImage);

                return peers;
            }
        }

        [SerializeField] private BoolReactiveProperty _opaque;

        [SerializeField] private ImageAndColorReactiveProperty _background;

        [SerializeField, HideInInspector] private Image _peerImage;

        protected override void InitializePeers()
        {
            base.InitializePeers();

            _peerImage = GetComponentInChildren<Image>();
        }

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            _opaque
                .Subscribe(v => PeerImage.enabled = v)
                .AddTo(this);
            _background
                .Subscribe(v => v.Update(PeerImage, DefaultBackground))
                .AddTo(this);
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            PeerImage.enabled = Opaque;
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            Background.Update(PeerImage, DefaultBackground);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Panel) component;

            Background = new ImageAndColor(source.Background);
            Opaque = source.Opaque;
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public static Panel CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Panel");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Panel>();
        }
    }
}