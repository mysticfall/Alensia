using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.UI.Event;
using Alensia.Core.UI.Property;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UEDropdown = UnityEngine.UI.Dropdown;

namespace Alensia.Core.UI
{
    public class Dropdown : InteractableComponent<UEDropdown, UEDropdown>,
        IInputComponent<string>, IPointerSelectionAware
    {
        public IReadOnlyList<DropdownItem> Items
        {
            get { return _items.Value; }
            set { _items.Value = value?.ToArray() ?? new DropdownItem[0]; }
        }

        public string Value
        {
            get { return PeerDropdown.value > -1 ? Items[PeerDropdown.value].Key : null; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                var index = Items.ToList().FindIndex(i => i.Key == value);

                PeerDropdown.value = index;
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

        public TextStyleSet ItemTextStyle
        {
            get { return _itemTextStyle.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _itemTextStyle.Value = value;
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

        public ImageAndColor PopupBackground
        {
            get { return _popupBackground.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _popupBackground.Value = value;
            }
        }

        public ImageAndColorSet ItemBackground
        {
            get { return _itemBackground.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _itemBackground.Value = value;
            }
        }

        public ImageAndColorSet ArrowImage
        {
            get { return _arrowImage.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _arrowImage.Value = value;
            }
        }

        public UniRx.IObservable<string> OnValueChange
        {
            get { return PeerDropdown.onValueChanged.AsObservable().Select(i => Items[i].Key); }
        }

        public UniRx.IObservable<IReadOnlyList<DropdownItem>> OnItemsChange
        {
            get { return _items.Select(i => (IReadOnlyList<DropdownItem>) i.ToList()); }
        }

        public UniRx.IObservable<PointerEventData> OnPointerPress => this.OnPointerDownAsObservable();

        public UniRx.IObservable<PointerEventData> OnPointerRelease => this.OnPointerUpAsObservable();

        public UniRx.IObservable<PointerEventData> OnPointerSelect => this.OnPointerClickAsObservable();

        protected override TextStyle DefaultTextStyle
        {
            get
            {
                var value = DefaultTextStyleSet;

                return value?.ValueFor(this)?.Merge(base.DefaultTextStyle) ?? base.DefaultTextStyle;
            }
        }

        protected TextStyleSet DefaultTextStyleSet => Style?.TextStyleSets?["Dropdown.Text"];

        protected TextStyle DefaultItemTextStyle
        {
            get
            {
                var value = DefaultItemTextStyleSet;

                return value?.ValueFor(this)?.Merge(base.DefaultTextStyle) ?? base.DefaultTextStyle;
            }
        }

        protected TextStyleSet DefaultItemTextStyleSet => Style?.TextStyleSets?["Dropdown.ItemText"];

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = DefaultBackgroundSet;

                return value?.ValueFor(this)?.Merge(base.DefaultBackground) ?? base.DefaultBackground;
            }
        }

        protected ImageAndColorSet DefaultBackgroundSet => Style?.ImageAndColorSets?["Dropdown.Background"];

        protected ImageAndColor DefaultPopupBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["Dropdown.PopupBackground"];

                return value == null ? base.DefaultBackground : value.Merge(base.DefaultBackground);
            }
        }

        protected ImageAndColor DefaultItemBackground
        {
            get
            {
                var value = DefaultItemBackgroundSet;

                return value?.ValueFor(this)?.Merge(base.DefaultBackground) ?? base.DefaultBackground;
            }
        }

        protected ImageAndColorSet DefaultItemBackgroundSet => Style?.ImageAndColorSets?["Dropdown.ItemBackground"];

        protected ImageAndColor DefaultArrowImage => DefaultArrowImageSet?.ValueFor(this);

        protected ImageAndColorSet DefaultArrowImageSet => Style?.ImageAndColorSets?["Dropdown.ArrowImage"];

        protected UEDropdown PeerDropdown => _peerDropdown ?? (_peerDropdown = GetComponent<UEDropdown>());

        protected Image PeerImage => _peerImage ?? (_peerImage = GetComponentInChildren<Image>());

        protected Text PeerLabel => _peerLabel ?? (_peerLabel = Transform.Find("Label").GetComponentInChildren<Text>());

        protected Image PeerArrow => _peerArrow ?? (_peerArrow = Transform.Find("Arrow").GetComponent<Image>());

        protected ScrollPanel PeerScrollPanel =>
            _peerScrollPanel ?? (_peerScrollPanel = GetComponentInChildren<ScrollPanel>(true));

        protected Toggle PeerToggle => _peerToggle ?? (_peerToggle = GetComponentInChildren<Toggle>(true));

        protected override UEDropdown PeerSelectable => PeerDropdown;

        protected override UEDropdown PeerHotspot => PeerDropdown;

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerDropdown != null) peers.Add(PeerDropdown);
                if (PeerImage != null) peers.Add(PeerImage);

                if (PeerLabel != null) peers.Add(PeerLabel.gameObject);
                if (PeerArrow != null) peers.Add(PeerArrow.gameObject);
                if (PeerScrollPanel != null) peers.Add(PeerScrollPanel.gameObject);

                return peers;
            }
        }

        [SerializeField] private DropdownItemList _items;

        [SerializeField] private TextStyleSetReactiveProperty _textStyle;

        [SerializeField] private TextStyleSetReactiveProperty _itemTextStyle;

        [SerializeField] private ImageAndColorSetReactiveProperty _background;

        [SerializeField] private ImageAndColorSetReactiveProperty _itemBackground;

        [SerializeField] private ImageAndColorSetReactiveProperty _arrowImage;

        [SerializeField] private ImageAndColorReactiveProperty _popupBackground;

        [SerializeField, HideInInspector] private UEDropdown _peerDropdown;

        [SerializeField, HideInInspector] private Image _peerImage;

        [SerializeField, HideInInspector] private Text _peerLabel;

        [SerializeField, HideInInspector] private Image _peerArrow;

        [SerializeField, HideInInspector] private ScrollPanel _peerScrollPanel;

        [SerializeField, HideInInspector] private Toggle _peerToggle;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            OnItemsChange
                .Subscribe(UpdateItems)
                .AddTo(this);

            _textStyle
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerDropdown.captionText, DefaultTextStyle))
                .AddTo(this);
            _itemTextStyle
                .Select(v => v.Merge(DefaultItemTextStyleSet))
                .Subscribe(v => PeerToggle.TextStyle = v)
                .AddTo(this);

            _background
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerImage, DefaultBackground))
                .AddTo(this);
            _popupBackground
                .Select(v => v.Merge(DefaultPopupBackground))
                .Subscribe(v => PeerScrollPanel.Background = v)
                .AddTo(this);
            _itemBackground
                .Select(v => v.Merge(DefaultItemBackgroundSet))
                .Subscribe(v => PeerToggle.Checkbox = v)
                .AddTo(this);
            _arrowImage
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerArrow, DefaultArrowImage))
                .AddTo(this);
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            TextStyle.ValueFor(this).Update(PeerDropdown.captionText, DefaultTextStyle);
            Background.ValueFor(this).Update(PeerImage, DefaultBackground);
            ArrowImage.ValueFor(this).Update(PeerArrow, DefaultArrowImage);
        }

        protected override void OnLocaleChanged(CultureInfo locale)
        {
            base.OnLocaleChanged(locale);

            UpdateItems(Items);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Dropdown) component;

            TextStyle = new TextStyleSet(source.TextStyle);
            ItemTextStyle = new TextStyleSet(source.ItemTextStyle);

            Background = new ImageAndColorSet(source.Background);
            ItemBackground = new ImageAndColorSet(source.ItemBackground);
            ArrowImage = new ImageAndColorSet(source.ArrowImage);

            PopupBackground = new ImageAndColor(source.PopupBackground);
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        private void UpdateItems(IEnumerable<DropdownItem> items)
        {
            var options = items.Select(i => i.AsOptionData(Context)).ToList();

            PeerDropdown.ClearOptions();
            PeerDropdown.AddOptions(options);
        }

        public static Dropdown CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Dropdown");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Dropdown>();
        }
    }

    [Serializable]
    internal class DropdownItemList : ReactiveProperty<DropdownItem[]>
    {
        public DropdownItemList() : base(new DropdownItem[0])
        {
        }

        public DropdownItemList(DropdownItem[] initialValue) : base(initialValue)
        {
        }
    }
}