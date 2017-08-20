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

        public UniRx.IObservable<string> OnValueChange
        {
            get { return PeerDropdown.onValueChanged.AsObservable().Select(i => Items[i].Key); }
        }

        public UniRx.IObservable<IReadOnlyList<DropdownItem>> OnItemsChange
        {
            get { return _items.Select(i => (IReadOnlyList<DropdownItem>) i.ToList()); }
        }

        protected UEDropdown PeerDropdown => _peerDropdown;

        protected Image PeerImage => _peerImage;

        protected Text PeerLabel => _peerLabel;

        protected Image PeerArrow => _peerArrow;

        protected ScrollRect PeerTemplate => _peerTemplate;

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

                return peers;
            }
        }

        [SerializeField] private DropdownItemList _items;

        [SerializeField] private TextStyleReactiveProperty _textStyle;

        [SerializeField] private TextStyleReactiveProperty _itemTextStyle;

        [SerializeField, HideInInspector] private UEDropdown _peerDropdown;

        [SerializeField, HideInInspector] private Image _peerImage;

        [SerializeField, HideInInspector] private Text _peerLabel;

        [SerializeField, HideInInspector] private Image _peerArrow;

        [SerializeField, HideInInspector] private ScrollRect _peerTemplate;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            OnItemsChange
                .Subscribe(UpdateItems)
                .AddTo(this);

            _textStyle
                .Subscribe(v => UpdatePeer(PeerDropdown.captionText, v))
                .AddTo(this);
            _itemTextStyle
                .Subscribe(v => UpdatePeer(PeerDropdown.itemText, v))
                .AddTo(this);
        }

        protected override void InitializePeers()
        {
            base.InitializePeers();

            _peerDropdown = GetComponent<UEDropdown>();
            _peerImage = GetComponentInChildren<Image>();
            _peerLabel = Transform.Find("Label").GetComponentInChildren<Text>();
            _peerArrow = Transform.Find("Arrow").GetComponent<Image>();
            _peerTemplate = Transform.Find("Template").GetComponentInChildren<ScrollRect>();
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            UpdateItems(_items.Value);

            UpdatePeer(PeerDropdown.captionText, TextStyle);
            UpdatePeer(PeerDropdown.itemText, ItemTextStyle);
        }

        protected override void OnLocaleChanged(CultureInfo locale)
        {
            base.OnLocaleChanged(locale);

            UpdateItems(_items.Value);

            UpdatePeer(PeerDropdown.captionText, TextStyle);
            UpdatePeer(PeerDropdown.itemText, ItemTextStyle);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Dropdown) component;

            TextStyle = new TextStyle(source.TextStyle);
            ItemTextStyle = new TextStyle(source.ItemTextStyle);
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