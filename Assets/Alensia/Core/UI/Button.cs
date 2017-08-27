using System.Collections.Generic;
using System.Globalization;
using Alensia.Core.I18n;
using Alensia.Core.UI.Event;
using Alensia.Core.UI.Property;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UEButton = UnityEngine.UI.Button;

namespace Alensia.Core.UI
{
    public class Button : InteractableComponent<UEButton, UEButton>, IPointerSelectionAware
    {
        public TranslatableText Text
        {
            get { return _text.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _text.Value = value;
            }
        }

        public TextStyle TextStyle
        {
            get { return _textStyle.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _textStyle.Value = value;
            }
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

        public IObservable<PointerEventData> OnPointerPress => this.OnPointerDownAsObservable();

        public IObservable<PointerEventData> OnPointerRelease => this.OnPointerUpAsObservable();

        public IObservable<PointerEventData> OnPointerSelect => this.OnPointerClickAsObservable();

        protected override TextStyle DefaultTextStyle
        {
            get
            {
                var value = Style?.TextStyles?["Button.Text"];

                return value == null ? base.DefaultTextStyle : value.Merge(base.DefaultTextStyle);
            }
        }

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["Button.Background"];

                return value == null ? base.DefaultBackground : value.Merge(base.DefaultBackground);
            }
        }

        protected UEButton PeerButton => _peerButton ?? (_peerButton = GetComponentInChildren<UEButton>());

        protected Text PeerText => _peerText ?? (_peerText = GetComponentInChildren<Text>());

        protected Image PeerImage => _peerImage ?? (_peerImage = GetComponentInChildren<Image>());

        protected override UEButton PeerSelectable => PeerButton;

        protected override UEButton PeerHotspot => PeerButton;

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerButton != null) peers.Add(PeerButton);
                if (PeerText != null) peers.Add(PeerText.gameObject);
                if (PeerImage != null) peers.Add(PeerImage);

                return peers;
            }
        }

        [SerializeField] private TranslatableTextReactiveProperty _text;

        [SerializeField] private TextStyleReactiveProperty _textStyle;

        [SerializeField] private ImageAndColorReactiveProperty _background;

        [SerializeField, HideInInspector] private UEButton _peerButton;

        [SerializeField, HideInInspector] private Image _peerImage;

        [SerializeField, HideInInspector] private Text _peerText;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            _text
                .Subscribe(v => UpdatePeer(PeerText, v))
                .AddTo(this);
            _textStyle
                .Subscribe(v => v.Update(PeerText, DefaultTextStyle))
                .AddTo(this);

            _background
                .Subscribe(v => v.Update(PeerImage, DefaultBackground))
                .AddTo(this);
        }

        protected override void OnLocaleChanged(CultureInfo locale)
        {
            base.OnLocaleChanged(locale);

            UpdatePeer(PeerText, Text);
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            TextStyle.Update(PeerText, DefaultTextStyle);
            Background.Update(PeerImage, DefaultBackground);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Button) component;

            _text.Value = new TranslatableText(source.Text);

            PeerText.text = source.Text.Text;

            TextStyle = new TextStyle(source.TextStyle);
            Background = new ImageAndColor(source.Background);
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public static Button CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Button");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Button>();
        }
    }
}