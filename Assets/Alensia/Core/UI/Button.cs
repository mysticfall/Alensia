using System;
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
using Object = UnityEngine.Object;
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

        public TextStyleSet TextStyle
        {
            get { return _textStyle.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _textStyle.Value = value;
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

        public ImageAndColorSet Icon
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

        public IObservable<PointerEventData> OnPointerPress =>
            this.OnPointerDownAsObservable().Where(_ => Interactable);

        public IObservable<PointerEventData> OnPointerRelease =>
            this.OnPointerUpAsObservable().Where(_ => Interactable);

        public IObservable<PointerEventData> OnPointerSelect =>
            this.OnPointerClickAsObservable().Where(_ => Interactable);

        protected override TextStyle DefaultTextStyle
        {
            get
            {
                var value = DefaultTextStyleSet;

                return value?.ValueFor(this)?.Merge(base.DefaultTextStyle) ?? base.DefaultTextStyle;
            }
        }

        protected virtual TextStyleSet DefaultTextStyleSet => Style?.TextStyleSets?["Button.Text"];

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = DefaultBackgroundSet;

                return value?.ValueFor(this)?.Merge(base.DefaultBackground) ?? base.DefaultBackground;
            }
        }

        protected virtual ImageAndColor DefaultIcon => DefaultIconSet?.ValueFor(this);

        protected virtual ImageAndColorSet DefaultIconSet => Style?.ImageAndColorSets?["Button.Icon"];

        protected virtual ImageAndColorSet DefaultBackgroundSet => Style?.ImageAndColorSets?["Button.Background"];

        protected UEButton PeerButton => _peerButton ?? (_peerButton = GetComponentInChildren<UEButton>());

        protected Text PeerText => _peerText ?? (_peerText = GetComponentInChildren<Text>());

        protected Image PeerBackground => _peerBackground ?? (_peerBackground = GetComponent<Image>());

        protected Image PeerIcon => _peerIcon ?? (_peerIcon = FindPeer<Image>("Icon"));

        protected HorizontalLayoutGroup LayoutGroup =>
            _layoutGroup ?? (_layoutGroup = GetComponent<HorizontalLayoutGroup>());

        protected LayoutElement IconLayout =>
            _iconLayout ?? (_iconLayout = PeerIcon?.GetComponent<LayoutElement>());

        protected override UEButton PeerSelectable => PeerButton;

        protected override UEButton PeerHotspot => PeerButton;

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerButton != null) peers.Add(PeerButton);
                if (PeerText != null) peers.Add(PeerText.gameObject);
                if (PeerBackground != null) peers.Add(PeerBackground);
                if (PeerIcon != null) peers.Add(PeerIcon.gameObject);

                if (LayoutGroup != null) peers.Add(LayoutGroup);

                return peers;
            }
        }

        [SerializeField] private TranslatableTextReactiveProperty _text;

        [SerializeField] private TextStyleSetReactiveProperty _textStyle;

        [SerializeField] private ImageAndColorSetReactiveProperty _background;

        [SerializeField] private ImageAndColorSetReactiveProperty _icon;

        [SerializeField] private Vector2ReactiveProperty _iconSize;

        [SerializeField, HideInInspector] private UEButton _peerButton;

        [SerializeField, HideInInspector] private Image _peerBackground;

        [SerializeField, HideInInspector] private Image _peerIcon;

        [SerializeField, HideInInspector] private Text _peerText;

        [SerializeField, HideInInspector] private HorizontalLayoutGroup _layoutGroup;

        [SerializeField, HideInInspector] private LayoutElement _iconLayout;

        protected override void InitializeComponent(IUIContext context, bool isPlaying)
        {
            base.InitializeComponent(context, isPlaying);

            if (!isPlaying) return;

            _text
                .Subscribe(UpdateText)
                .AddTo(this);
            _textStyle
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerText, DefaultTextStyle))
                .AddTo(this);

            _background
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerBackground, DefaultBackground))
                .AddTo(this);
            _icon
                .Select(v => v.ValueFor(this))
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

        protected override void OnLocaleChanged(CultureInfo locale)
        {
            base.OnLocaleChanged(locale);

            UpdateText(Text);
        }

        protected override void OnEditorUpdate()
        {
            base.OnEditorUpdate();

            UpdateText(Text);
            UpdateIcon(Icon.ValueFor(this));

            IconLayout.preferredWidth = IconSize.x;
            IconLayout.preferredHeight = IconSize.y;
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            TextStyle.ValueFor(this).Update(PeerText, DefaultTextStyle);
            Background.ValueFor(this).Update(PeerBackground, DefaultBackground);

            UpdateIcon(Icon.ValueFor(this));
        }

        private void UpdateText(TranslatableText text)
        {
            if (PeerText == null) return;

            UpdatePeer(PeerText, text);

            var empty = string.IsNullOrEmpty(text.Text) && string.IsNullOrEmpty(text.TextKey);

            PeerText.gameObject.SetActive(!empty);
        }

        private void UpdateIcon(ImageAndColor icon)
        {
            if (PeerIcon == null) return;

            icon.Update(PeerIcon, DefaultIcon);

            PeerIcon.gameObject.SetActive(icon.Image.HasValue);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Button) component;

            _text.Value = new TranslatableText(source.Text);

            PeerText.text = source.Text.Text;

            TextStyle = new TextStyleSet(source.TextStyle);

            Background = new ImageAndColorSet(source.Background);
            Icon = new ImageAndColorSet(source.Icon);
            IconSize = new Vector2(source.IconSize.x, source.IconSize.y);
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