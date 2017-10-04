using System;
using System.Collections.Generic;
using System.Globalization;
using Alensia.Core.I18n;
using Alensia.Core.Interaction.Event;
using Alensia.Core.UI.Property;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Alensia.Core.UI
{
    public class ToggleButton : InteractableComponent<Toggle, Toggle>,
        IInputComponent<bool>, IPointerSelectionAware
    {
        public bool Value
        {
            get { return PeerToggle.isOn; }
            set { PeerToggle.isOn = value; }
        }

        public TranslatableText Text
        {
            get { return _text.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _text.Value = value;
            }
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

        public ImageAndColorSet Checkmark
        {
            get { return _checkmark.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _checkmark.Value = value;
            }
        }

        public ImageAndColorSet Checkbox
        {
            get { return _checkbox.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _checkbox.Value = value;
            }
        }

        public ToggleGroup ToggleGroup
        {
            get { return _toggleGroup.Value; }
            set { _toggleGroup.Value = value; }
        }

        public IObservable<bool> OnValueChange => PeerToggle.OnValueChangedAsObservable();

        public IObservable<PointerEventData> OnPointerPress => this.OnPointerDownAsObservable();

        public IObservable<PointerEventData> OnPointerRelease => this.OnPointerUpAsObservable();

        public IObservable<PointerEventData> OnPointerSelect => this.OnPointerClickAsObservable();

        protected override TextStyle DefaultTextStyle
        {
            get
            {
                var value = DefaultTextStyleSet;

                return value?.ValueFor(!Interactable, Highlighted, Value)?.Merge(base.DefaultTextStyle) ??
                       base.DefaultTextStyle;
            }
        }

        protected TextStyleSet DefaultTextStyleSet => Style?.TextStyleSets?["Toggle.Text"];

        protected ImageAndColor DefaultCheckmark => DefaultCheckmarkSet?.ValueFor(!Interactable, Highlighted, Value);

        protected ImageAndColorSet DefaultCheckmarkSet => Style?.ImageAndColorSets?["Toggle.Checkmark"];

        protected ImageAndColor DefaultCheckbox => DefaultCheckboxSet?.ValueFor(!Interactable, Highlighted, Value);

        protected ImageAndColorSet DefaultCheckboxSet => Style?.ImageAndColorSets?["Toggle.Checkbox"];

        protected Toggle PeerToggle => _peerToggle ?? (_peerToggle = GetComponentInChildren<Toggle>());

        protected Text PeerText => _peerText ?? (_peerText = GetComponentInChildren<Text>());

        protected Image PeerCheckbox => _peerCheckbox ?? (_peerCheckbox = PeerToggle.image);

        protected Image PeerCheckmark =>
            _peerCheckmark ?? (_peerCheckmark = PeerToggle.graphic.GetComponent<Image>());

        protected Transform PeerBackground => _peerBackground ?? (_peerBackground = Transform.Find("Background"));

        protected override Toggle PeerSelectable => PeerToggle;

        protected override Toggle PeerHotspot => PeerToggle;

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerToggle != null) peers.Add(PeerToggle);
                if (PeerText != null) peers.Add(PeerText.gameObject);
                if (PeerBackground != null) peers.Add(PeerBackground.gameObject);

                return peers;
            }
        }

        [SerializeField] private TranslatableTextReactiveProperty _text;

        [SerializeField] private TextStyleSetReactiveProperty _textStyle;

        [SerializeField] private ImageAndColorSetReactiveProperty _checkmark;

        [SerializeField] private ImageAndColorSetReactiveProperty _checkbox;

        [SerializeField] private ToggleGroupReactiveProperty _toggleGroup;

        [SerializeField, HideInInspector] private Toggle _peerToggle;

        [SerializeField, HideInInspector] private Image _peerCheckbox;

        [SerializeField, HideInInspector] private Image _peerCheckmark;

        [SerializeField, HideInInspector] private Text _peerText;

        [NonSerialized] private Transform _peerBackground;

        protected override void InitializeComponent(IUIContext context, bool isPlaying)
        {
            base.InitializeComponent(context, isPlaying);

            if (!isPlaying) return;

            _text
                .Subscribe(v => UpdatePeer(PeerText, v), Debug.LogError)
                .AddTo(this);
            _textStyle
                .Select(v => v.ValueFor(!Interactable, Highlighted, Value))
                .Subscribe(v => v.Update(PeerText, DefaultTextStyle), Debug.LogError)
                .AddTo(this);

            _checkmark
                .Select(v => v.ValueFor(!Interactable, Highlighted, Value))
                .Subscribe(v => v.Update(PeerCheckmark, DefaultCheckmark), Debug.LogError)
                .AddTo(this);
            _checkbox
                .Select(v => v.ValueFor(!Interactable, Highlighted, Value))
                .Subscribe(v => v.Update(PeerCheckbox, DefaultCheckbox), Debug.LogError)
                .AddTo(this);
            _toggleGroup
                .Subscribe(v => PeerToggle.group = v, Debug.LogError)
                .AddTo(this);

            OnValueChange
                .Select(_ => Style)
                .Where(v => v != null)
                .Subscribe(_ => OnStyleChanged(Style), Debug.LogError)
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

            TextStyle.ValueFor(!Interactable, Highlighted, Value).Update(PeerText, DefaultTextStyle);
            Checkmark.ValueFor(!Interactable, Highlighted, Value).Update(PeerCheckmark, DefaultCheckmark);
            Checkbox.ValueFor(!Interactable, Highlighted, Value).Update(PeerCheckbox, DefaultCheckbox);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (ToggleButton) component;

            _text.Value = new TranslatableText(source.Text);

            PeerText.text = source.Text.Text;

            TextStyle = new TextStyleSet(source.TextStyle);
            Checkmark = new ImageAndColorSet(source.Checkmark);
            Checkbox = new ImageAndColorSet(source.Checkbox);

            ToggleGroup = null;
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public static ToggleButton CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/ToggleButton");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<ToggleButton>();
        }

        [Serializable]
        internal class ToggleGroupReactiveProperty : ReactiveProperty<ToggleGroup>
        {
        }
    }
}