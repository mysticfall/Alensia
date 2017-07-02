using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    [RequireComponent(typeof(Text))]
    public class Label : UIComponent
    {
        public string Text
        {
            get { return _text.Value; }
            set { _text.Value = value; }
        }

        public string TextKey
        {
            get { return _textKey.Value; }
            set { _textKey.Value = value; }
        }

        protected Text PeerText { get; private set; }

        [SerializeField] private StringReactiveProperty _text =
            new StringReactiveProperty("Label");

        [SerializeField] private StringReactiveProperty _textKey;

        protected override void OnValidate()
        {
            base.OnValidate();

            PeerText = PeerText ?? GetComponentInChildren<Text>();

            Assert.IsNotNull(PeerText, "Missing Text component.");

            PeerText.text = Text;
        }

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            var localeService = context.Translator.LocaleService;

            localeService
                .OnLocaleChange
                .Select(_ => TextKey)
                .Merge(_textKey)
                .Select(key => key == null ? null : Context.Translator[key])
                .Subscribe(text => PeerText.text = text);
        }

        public static Label CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Label");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Label>();
        }
    }
}