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

        protected virtual string DefaultText => "Label";

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

        protected override void Reset()
        {
            base.Reset();

            _text.Value = new TranslatableText(DefaultText);

            PeerText.text = DefaultText;

            var source = CreateInstance();

            TextStyle.Load(source.PeerText);
            TextStyle.Update(PeerText);

            DestroyImmediate(source.gameObject);
        }

        public static Label CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Label");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Label>();
        }
    }
}