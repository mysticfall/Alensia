using System.Collections.Generic;
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

        protected Text PeerText => _peerText;

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

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            var localeService = context.Translator.LocaleService;

            localeService
                .OnLocaleChange
                .Select(_ => Text)
                .Merge(_text)
                .Select(text => text.Translate(Context.Translator))
                .Subscribe(text => PeerText.text = text)
                .AddTo(this);

            _textStyle
                .Subscribe(s => s.Update(PeerText))
                .AddTo(this);
        }

        protected override void InitializePeers()
        {
            base.InitializePeers();

            _peerText = GetComponentInChildren<Text>();
        }

        protected override void ValidateProperties()
        {
            base.ValidateProperties();

            PeerText.text = Text.Text;

            TextStyle.Update(PeerText);
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