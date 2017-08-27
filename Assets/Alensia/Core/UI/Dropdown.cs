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
    public class Dropdown : UIComponent, IInputComponent<string>, IPointerSelectionAware
    {
        public bool Interactable
        {
            get { return _interactable.Value; }
            set { _interactable.Value = value; }
        }

        public bool Interacting => _interactionTracker != null && _interactionTracker.State;

        public bool Highlighted => _highlightTracker != null && _highlightTracker.State;

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

        public UniRx.IObservable<bool> OnInteractableStateChange => _interactable;

        public UniRx.IObservable<bool> OnInteractingStateChange => _interactionTracker?.OnStateChange;

        public UniRx.IObservable<bool> OnHighlightedStateChange => _highlightTracker?.OnStateChange;

        public UniRx.IObservable<PointerEventData> OnPointerPress => this.OnPointerDownAsObservable();

        public UniRx.IObservable<PointerEventData> OnPointerRelease => this.OnPointerUpAsObservable();

        public UniRx.IObservable<PointerEventData> OnPointerSelect => this.OnPointerClickAsObservable();

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

        protected UEDropdown PeerDropdown => _peerDropdown ?? (_peerDropdown = GetComponent<UEDropdown>());

        protected Image PeerImage => _peerImage ?? (_peerImage = GetComponentInChildren<Image>());

        protected Text PeerLabel => _peerLabel ?? (_peerLabel = Transform.Find("Label").GetComponentInChildren<Text>());

        protected Image PeerArrow => _peerArrow ?? (_peerArrow = Transform.Find("Arrow").GetComponent<Image>());

        protected ScrollRect PeerTemplate => _peerTemplate ??
                                             (_peerTemplate = Transform
                                                 .Find("Template")
                                                 .GetComponentInChildren<ScrollRect>());

        protected Image PeerPopupImage => _peerPopupImage ??
                                          (_peerPopupImage = Transform
                                              .Find("Template")
                                              .GetComponentInChildren<Image>());

        protected Image PeerItemImage => _peerItemImage ??
                                         (_peerItemImage = Transform
                                             .Find("Template")
                                             .Find("Viewport/Content/Item/Item Background")
                                             .GetComponentInChildren<Image>());

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

        [SerializeField] private BoolReactiveProperty _interactable;

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

        private EventTracker<Dropdown> _interactionTracker;

        private EventTracker<Dropdown> _highlightTracker;

        private List<EventTracker<Dropdown>> _trackers;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            _interactionTracker = new PointerSelectionTracker<Dropdown>(this);
            _highlightTracker = new PointerPresenceTracker<Dropdown>(this);

            _trackers = new List<EventTracker<Dropdown>> {_interactionTracker, _highlightTracker};

            _trackers.ForEach(t => t.Initialize());

            _interactable
                .Subscribe(v =>
                {
                    PeerDropdown.interactable = v;
                    _trackers.ForEach(t => t.Active = v);
                })
                .AddTo(this);

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

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _trackers?.ForEach(t => t.Dispose());
            _trackers = null;
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