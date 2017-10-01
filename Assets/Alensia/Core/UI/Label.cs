using System.Collections.Generic;
using System.Globalization;
using Alensia.Core.I18n;
using Alensia.Core.UI.Property;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    public class Label : UIComponent
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

        protected override TextStyle DefaultTextStyle
        {
            get
            {
                var value = Style?.TextStyles?["Label.Text"];

                return value == null ? base.DefaultTextStyle : value.Merge(base.DefaultTextStyle);
            }
        }

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["Label.Background"];

                return value == null ? base.DefaultBackground : value.Merge(base.DefaultBackground);
            }
        }

        protected Text PeerText => _peerText ?? (_peerText = GetComponentInChildren<Text>());

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerText != null) peers.Add(PeerText);

                return peers;
            }
        }

        [SerializeField] private TranslatableTextReactiveProperty _text;

        [SerializeField] private TextStyleReactiveProperty _textStyle;

        [SerializeField, HideInInspector] private Text _peerText;

        protected override void InitializeComponent(IUIContext context, bool isPlaying)
        {
            base.InitializeComponent(context, isPlaying);

            if (!isPlaying) return;

            _text
                .Subscribe(v => UpdatePeer(PeerText, v))
                .AddTo(this);
            _textStyle
                .Subscribe(v => v.Update(PeerText, DefaultTextStyle))
                .AddTo(this);
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            TextStyle.Update(PeerText, DefaultTextStyle);
        }

        protected override void OnLocaleChanged(CultureInfo locale)
        {
            base.OnLocaleChanged(locale);

            UpdatePeer(PeerText, Text);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Label) component;

            _text.Value = new TranslatableText(source.Text);

            PeerText.text = source.Text.Text;

            TextStyle = new TextStyle(source.TextStyle);
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public static Label CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Label");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Label>();
        }
    }
}