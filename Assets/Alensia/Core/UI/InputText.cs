using System.Collections.Generic;
using System.Globalization;
using Alensia.Core.I18n;
using Alensia.Core.UI.Event;
using Alensia.Core.UI.Property;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    public class InputText : InteractableComponent<InputField, InputField>, IInputComponent<string>
    {
        public string Value
        {
            get { return _value.Value; }
            set { _value.Value = value; }
        }

        public TranslatableText PlaceholderText
        {
            get { return _placeholderText.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _placeholderText.Value = value;
            }
        }

        public bool ReadOnly
        {
            get { return _readOnly.Value; }
            set { _readOnly.Value = value; }
        }

        public int CharacterLimit
        {
            get { return _characterLimit.Value; }
            set { _characterLimit.Value = value; }
        }

        public int CaretWidth
        {
            get { return _caretWidth.Value; }
            set { _caretWidth.Value = value; }
        }

        public TextStyleSet TextStyle
        {
            get { return _textStyle.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _textStyle.Value = value;
            }
        }

        public TextStyleSet PlaceholderTextStyle
        {
            get { return _placeholderTextStyle.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _placeholderTextStyle.Value = value;
            }
        }

        public ImageAndColorSet Background
        {
            get { return _background.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _background.Value = value;
            }
        }

        public UnsettableColor SelectionColor
        {
            get { return _selectionColor.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _selectionColor.Value = value;
            }
        }

        public UnsettableColor CaretColor
        {
            get { return _caretColor.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _caretColor.Value = value;
            }
        }

        public IObservable<string> OnEdit => PeerInput.OnEndEditAsObservable();

        public IObservable<string> OnValueChange => _value;

        protected override TextStyle DefaultTextStyle
        {
            get
            {
                var value = DefaultTextStyleSet;

                return value?.ValueFor(this)?.Merge(base.DefaultTextStyle) ?? base.DefaultTextStyle;
            }
        }

        protected virtual TextStyleSet DefaultTextStyleSet => Style?.TextStyleSets?["InputText.Text"];

        protected virtual TextStyle DefaultPlaceholderStyle
        {
            get
            {
                var value = DefaultPlaceholderStyleSet;

                return value?.ValueFor(this)?.Merge(base.DefaultTextStyle) ?? base.DefaultTextStyle;
            }
        }

        protected virtual TextStyleSet DefaultPlaceholderStyleSet => Style?.TextStyleSets?["InputText.Placeholder"];

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = DefaultBackgroundSet;

                return value?.ValueFor(this)?.Merge(base.DefaultBackground) ?? base.DefaultBackground;
            }
        }

        protected virtual ImageAndColorSet DefaultBackgroundSet => Style?.ImageAndColorSets?["InputText.Background"];

        protected virtual UnsettableColor DefaultSelectionColor => Style?.Colors?["InputText.Selection"];

        protected virtual UnsettableColor DefaultCaretColor => Style?.Colors?["InputText.Caret"];

        protected InputField PeerInput => _peerInput ?? (_peerInput = GetComponent<InputField>());

        protected Text PeerText => _peerText ?? (_peerText = FindPeer<Text>("Text"));

        protected Text PeerPlaceholder => _peerPlaceholder ?? (_peerPlaceholder = FindPeer<Text>("Placeholder"));

        protected Image PeerBackground => _peerBackground ?? (_peerBackground = GetComponent<Image>());

        protected override InputField PeerSelectable => PeerInput;

        protected override InputField PeerHotspot => PeerInput;

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerInput != null) peers.Add(PeerInput);
                if (PeerText != null) peers.Add(PeerText.gameObject);
                if (PeerPlaceholder != null) peers.Add(PeerPlaceholder.gameObject);
                if (PeerBackground != null) peers.Add(PeerBackground);

                return peers;
            }
        }

        [SerializeField] private StringReactiveProperty _value;

        [SerializeField] private TranslatableTextReactiveProperty _placeholderText;

        [SerializeField] private BoolReactiveProperty _readOnly;

        [SerializeField] private IntReactiveProperty _characterLimit;

        [SerializeField, RangeReactiveProperty(1, 5)] private IntReactiveProperty _caretWidth;

        [SerializeField] private TextStyleSetReactiveProperty _textStyle;

        [SerializeField] private TextStyleSetReactiveProperty _placeholderTextStyle;

        [SerializeField] private UnsettableColorReactiveProperty _selectionColor;

        [SerializeField] private UnsettableColorReactiveProperty _caretColor;

        [SerializeField] private ImageAndColorSetReactiveProperty _background;

        [SerializeField, HideInInspector] private InputField _peerInput;

        [SerializeField, HideInInspector] private Image _peerBackground;

        [SerializeField, HideInInspector] private Text _peerText;

        [SerializeField, HideInInspector] private Text _peerPlaceholder;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            _value
                .Subscribe(v => PeerInput.text = v)
                .AddTo(this);
        }

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            PeerInput
                .OnValueChangedAsObservable()
                .Subscribe(v => Value = v)
                .AddTo(this);

            _readOnly
                .Subscribe(v => PeerInput.readOnly = v)
                .AddTo(this);
            _characterLimit
                .Subscribe(v => PeerInput.characterLimit = v)
                .AddTo(this);
            _caretWidth
                .Subscribe(v => PeerInput.caretWidth = v)
                .AddTo(this);

            _placeholderText
                .Subscribe(UpdatePlaceholder)
                .AddTo(this);

            _textStyle
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerText, DefaultTextStyle))
                .AddTo(this);
            _placeholderTextStyle
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerPlaceholder, DefaultPlaceholderStyle))
                .AddTo(this);

            _background
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerBackground, DefaultBackground))
                .AddTo(this);
            _selectionColor
                .Select(v => v.OrDefault(DefaultSelectionColor))
                .Subscribe(v => PeerInput.selectionColor = v)
                .AddTo(this);
            _caretColor
                .Select(v => v.HasValue ? v : DefaultCaretColor)
                .Subscribe(v =>
                {
                    PeerInput.customCaretColor = v.HasValue;
                    PeerInput.caretColor = v.OrDefault(Color.black);
                })
                .AddTo(this);
        }

        protected override void OnLocaleChanged(CultureInfo locale)
        {
            base.OnLocaleChanged(locale);

            UpdatePlaceholder(PlaceholderText);
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            UpdatePlaceholder(PlaceholderText);
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            TextStyle.ValueFor(this).Update(PeerText, DefaultTextStyle);
            PlaceholderTextStyle.ValueFor(this).Update(PeerPlaceholder, DefaultPlaceholderStyle);

            Background.ValueFor(this).Update(PeerBackground, DefaultBackground);

            PeerInput.selectionColor = SelectionColor.OrDefault(DefaultSelectionColor);
            PeerInput.caretColor = CaretColor.OrDefault(DefaultCaretColor);
            PeerInput.customCaretColor = CaretColor.HasValue || DefaultCaretColor != null && DefaultCaretColor.HasValue;
        }

        private void UpdatePlaceholder(TranslatableText text)
        {
            if (PeerPlaceholder == null) return;

            UpdatePeer(PeerPlaceholder, text);

            var empty = string.IsNullOrEmpty(text.Text) && string.IsNullOrEmpty(text.TextKey);

            PeerPlaceholder.gameObject.SetActive(!empty);
        }

        protected override EventTracker<InputField> CreateInterationTracker() =>
            new FocusTracker<InputField>(PeerHotspot);

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (InputText) component;

            Value = source.Value;
            PlaceholderText = source.PlaceholderText;

            ReadOnly = source.ReadOnly;
            CharacterLimit = source.CharacterLimit;
            CaretWidth = source.CaretWidth;

            TextStyle = new TextStyleSet(source.TextStyle);
            PlaceholderTextStyle = new TextStyleSet(source.PlaceholderTextStyle);

            Background = new ImageAndColorSet(source.Background);
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public static InputText CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/InputText");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<InputText>();
        }
    }
}