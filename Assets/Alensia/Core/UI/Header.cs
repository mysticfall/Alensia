using System.Collections.Generic;
using Alensia.Core.UI.Property;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    public class Header : Label
    {
        public ImageAndColor Background
        {
            get { return _background.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _background.Value = value;
            }
        }

        public ImageAndColor Icon
        {
            get { return _icon.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _icon.Value = value;
            }
        }

        public Vector2 IconSize
        {
            get { return _iconSize.Value; }
            set { _iconSize.Value = value; }
        }

        public Transform ControlPanel => _controlPanel ?? (_controlPanel = Transform.Find("Controls"));

        public IList<IComponent> Controls => ControlPanel.GetComponentsInChildren<IComponent>();

        protected override TextStyle DefaultTextStyle
        {
            get
            {
                var value = Style?.TextStyles?["Header.Text"];

                return value == null ? base.DefaultTextStyle : value.Merge(base.DefaultTextStyle);
            }
        }

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["Header.Background"];

                return value == null ? base.DefaultBackground : value.Merge(base.DefaultBackground);
            }
        }

        protected virtual ImageAndColor DefaultIcon => Style?.ImagesAndColors?["Header.Icon"];

        protected HorizontalLayoutGroup LayoutGroup =>
            _layoutGroup ?? (_layoutGroup = GetComponent<HorizontalLayoutGroup>());

        protected LayoutElement ControlLayout =>
            _controlLayout ?? (_controlLayout = ControlPanel.GetComponent<LayoutElement>());

        protected HorizontalLayoutGroup ControlLayoutGroup =>
            _controlLayoutGroup ?? (_controlLayoutGroup = ControlPanel.GetComponent<HorizontalLayoutGroup>());

        protected LayoutElement IconLayout => _iconLayout ?? (_iconLayout = PeerIcon.GetComponent<LayoutElement>());

        protected Image PeerBackground => _peerBackground ?? (_peerBackground = FindPeer<Image>("Background"));

        protected Image PeerIcon => _peerIcon ?? (_peerIcon = FindPeer<Image>("Icon"));

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerIcon != null) peers.Add(PeerIcon.gameObject);
                if (PeerText != null) peers.Add(PeerText.gameObject);
                if (PeerBackground != null) peers.Add(PeerBackground.gameObject);

                if (LayoutGroup != null) peers.Add(LayoutGroup);
                if (ControlLayout != null) peers.Add(ControlLayout);
                if (ControlLayoutGroup != null) peers.Add(ControlLayoutGroup);

                return peers;
            }
        }

        [SerializeField] private ImageAndColorReactiveProperty _background;

        [SerializeField] private ImageAndColorReactiveProperty _icon;

        [SerializeField] private Vector2ReactiveProperty _iconSize;

        [SerializeField, HideInInspector] private Image _peerBackground;

        [SerializeField, HideInInspector] private Image _peerIcon;

        [SerializeField, HideInInspector] private HorizontalLayoutGroup _layoutGroup;

        [SerializeField, HideInInspector] private HorizontalLayoutGroup _controlLayoutGroup;

        [SerializeField, HideInInspector] private LayoutElement _iconLayout;

        [SerializeField, HideInInspector] private LayoutElement _controlLayout;

        private Transform _controlPanel;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            if (!Application.isPlaying) return;

            foreach (var control in Controls)
            {
                control.Initialize(context);
            }
        }

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            _background
                .Subscribe(v => v.Update(PeerBackground, DefaultBackground))
                .AddTo(this);
            _icon
                .Subscribe(UpdateIcon)
                .AddTo(this);
            _iconSize
                .Subscribe(v =>
                {
                    IconLayout.preferredWidth = v.x;
                    IconLayout.preferredHeight = v.y;
                })
                .AddTo(this);
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            UpdateIcon(Icon);

            IconLayout.preferredWidth = IconSize.x;
            IconLayout.preferredHeight = IconSize.y;
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            Background.Update(PeerBackground, DefaultBackground);

            UpdateIcon(Icon);
        }

        private void UpdateIcon(ImageAndColor icon)
        {
            icon.Update(PeerIcon, DefaultIcon);

            var hasIcon = icon.Image.HasValue;

            LayoutGroup.padding.left = hasIcon ? 5 : 10;

            PeerIcon.gameObject.SetActive(hasIcon);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Header) component;

            Icon = new ImageAndColor(source.Icon);
            IconSize = new Vector2(source.IconSize.x, source.IconSize.y);
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public new static Header CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Header");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Header>();
        }
    }
}