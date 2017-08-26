using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.UI.Property;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UEDropdown = UnityEngine.UI.Dropdown;

namespace Alensia.Core.UI
{
    public class Dropdown : UIComponent, IInputComponent<string>
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

        public TextStyle TextStyle
        {
            get { return _textStyle.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _textStyle.Value = value;
            }
        }

        public TextStyle ItemTextStyle
        {
            get { return _itemTextStyle.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _itemTextStyle.Value = value;
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

        public ImageAndColor PopupBackground
        {
            get { return _popupBackground.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _popupBackground.Value = value;
            }
        }

        public ImageAndColor ItemBackground
        {
            get { return _itemBackground.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _itemBackground.Value = value;
            }
        }

        public ImageAndColor ArrowImage
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

        protected override TextStyle DefaultTextStyle
        {
            get
            {
                var value = Style?.TextStyles?["Dropdown.Text"];

                return value == null ? base.DefaultTextStyle : value.Merge(base.DefaultTextStyle);
            }
        }

        protected TextStyle DefaultItemTextStyle
        {
            get
            {
                var value = Style?.TextStyles?["Dropdown.ItemText"];

                return value == null ? DefaultTextStyle : value.Merge(DefaultTextStyle);
            }
        }

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["Dropdown.Background"];

                return value == null ? base.DefaultBackground : value.Merge(base.DefaultBackground);
            }
        }

        protected ImageAndColor DefaultPopupBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["Dropdown.PopupBackground"];

                return value == null ? DefaultBackground : value.Merge(DefaultBackground);
            }
        }

        protected ImageAndColor DefaultItemBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["Dropdown.ItemBackground"];

                return value == null ? DefaultPopupBackground : value.Merge(DefaultPopupBackground);
            }
        }

        protected ImageAndColor DefaultArrowImage
        {
            get
            {
                var value = Style?.ImagesAndColors?["Dropdown.ArrowImage"];

                return value == null ? DefaultBackground : value.Merge(DefaultBackground);
            }
        }

        protected UEDropdown PeerDropdown => _peerDropdown;

        protected Image PeerImage => _peerImage;

        protected Text PeerLabel => _peerLabel;

        protected Image PeerArrow => _peerArrow;

        protected ScrollRect PeerTemplate => _peerTemplate;

        protected Image PeerPopupImage => _peerPopupImage;

        protected Image PeerItemImage => _peerItemImage;

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerDropdown != null) peers.Add(PeerDropdown);
                if (PeerImage != null) peers.Add(PeerImage);

                if (PeerLabel != null) peers.Add(PeerLabel.gameObject);
                if (PeerArrow != null) peers.Add(PeerArrow.gameObject);
                if (PeerTemplate != null) peers.Add(PeerTemplate.gameObject);

                if (PeerPopupImage != null) peers.Add(PeerPopupImage.gameObject);
                if (PeerItemImage != null) peers.Add(PeerItemImage.gameObject);

                return peers;
            }
        }

        [SerializeField] private DropdownItemList _items;

        [SerializeField] private TextStyleReactiveProperty _textStyle;

        [SerializeField] private TextStyleReactiveProperty _itemTextStyle;

        [SerializeField] private ImageAndColorReactiveProperty _background;

        [SerializeField] private ImageAndColorReactiveProperty _popupBackground;

        [SerializeField] private ImageAndColorReactiveProperty _itemBackground;

        [SerializeField] private ImageAndColorReactiveProperty _arrowImage;

        [SerializeField, HideInInspector] private UEDropdown _peerDropdown;

        [SerializeField, HideInInspector] private Image _peerImage;

        [SerializeField, HideInInspector] private Text _peerLabel;

        [SerializeField, HideInInspector] private Image _peerArrow;

        [SerializeField, HideInInspector] private Image _peerPopupImage;

        [SerializeField, HideInInspector] private Image _peerItemImage;

        [SerializeField, HideInInspector] private ScrollRect _peerTemplate;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            OnItemsChange
                .Subscribe(UpdateItems)
                .AddTo(this);

            _textStyle
                .Subscribe(v => v.Update(PeerDropdown.captionText, DefaultTextStyle))
                .AddTo(this);
            _itemTextStyle
                .Subscribe(v => v.Update(PeerDropdown.itemText, DefaultItemTextStyle))
                .AddTo(this);

            _background
                .Subscribe(v => v.Update(PeerImage, DefaultBackground))
                .AddTo(this);
            _popupBackground
                .Subscribe(v => v.Update(PeerPopupImage, DefaultPopupBackground))
                .AddTo(this);
            _itemBackground
                .Subscribe(v => v.Update(PeerItemImage, DefaultItemBackground))
                .AddTo(this);
            _arrowImage
                .Subscribe(v => v.Update(PeerArrow, DefaultArrowImage))
                .AddTo(this);
        }

        protected override void InitializePeers()
        {
            base.InitializePeers();

            _peerDropdown = GetComponent<UEDropdown>();
            _peerImage = GetComponentInChildren<Image>();
            _peerLabel = Transform.Find("Label").GetComponentInChildren<Text>();
            _peerArrow = Transform.Find("Arrow").GetComponent<Image>();

            var template = Transform.Find("Template");

            _peerTemplate = template.GetComponentInChildren<ScrollRect>();
            _peerPopupImage = template.GetComponentInChildren<Image>();
            _peerItemImage = template
                .Find("Viewport/Content/Item/Item Background")
                .GetComponentInChildren<Image>();
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            TextStyle.Update(PeerDropdown.captionText, DefaultTextStyle);
            ItemTextStyle.Update(PeerDropdown.itemText, DefaultItemTextStyle);

            Background.Update(PeerImage, DefaultBackground);
            PopupBackground.Update(PeerPopupImage, DefaultPopupBackground);
            ItemBackground.Update(PeerItemImage, DefaultItemBackground);
            ArrowImage.Update(PeerArrow, DefaultArrowImage);
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

            TextStyle = new TextStyle(source.TextStyle);
            ItemTextStyle = new TextStyle(source.ItemTextStyle);

            Background = new ImageAndColor(source.Background);
            PopupBackground = new ImageAndColor(source.PopupBackground);
            ItemBackground = new ImageAndColor(source.ItemBackground);
            ArrowImage = new ImageAndColor(source.ArrowImage);
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