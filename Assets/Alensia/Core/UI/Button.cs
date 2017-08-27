using System.Collections.Generic;
using Alensia.Core.UI.Event;
using Alensia.Core.UI.Property;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UEButton = UnityEngine.UI.Button;

namespace Alensia.Core.UI
{
    public class Button : Label, IInteractableComponent, IPointerSelectionAware
    {
        public bool Interactable
        {
            get { return _interactable.Value; }
            set { _interactable.Value = value; }
        }

        public bool Interacting => _interactionTracker != null && _interactionTracker.State;

        public bool Highlighted => _highlightTracker != null && _highlightTracker.State;

        public ImageAndColor Background
        {
            get { return _background.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _background.Value = value;
            }
        }

        public IObservable<bool> OnInteractableStateChange => _interactable;

        public IObservable<bool> OnInteractingStateChange => _interactionTracker?.OnStateChange;

        public IObservable<bool> OnHighlightedStateChange => _highlightTracker?.OnStateChange;

        public IObservable<PointerEventData> OnPointerPress => this.OnPointerDownAsObservable();

        public IObservable<PointerEventData> OnPointerRelease => this.OnPointerUpAsObservable();

        public IObservable<PointerEventData> OnPointerSelect => this.OnPointerClickAsObservable();

        protected override TextStyle DefaultTextStyle
        {
            get
            {
                var value = Style?.TextStyles?["Button.Text"];

                return value == null ? base.DefaultTextStyle : value.Merge(base.DefaultTextStyle);
            }
        }

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["Button.Background"];

                return value == null ? base.DefaultBackground : value.Merge(base.DefaultBackground);
            }
        }

        protected UEButton PeerButton => _peerButton ?? (_peerButton = GetComponentInChildren<UEButton>());

        protected Image PeerImage => _peerImage ?? (_peerImage = GetComponentInChildren<Image>());

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerButton != null) peers.Add(PeerButton);
                if (PeerImage != null) peers.Add(PeerImage);

                if (PeerText != null) peers.Add(PeerText.gameObject);

                return peers;
            }
        }

        [SerializeField] private BoolReactiveProperty _interactable;

        [SerializeField] private ImageAndColorReactiveProperty _background;

        [SerializeField, HideInInspector] private UEButton _peerButton;

        [SerializeField, HideInInspector] private Image _peerImage;

        private EventTracker<Button> _interactionTracker;

        private EventTracker<Button> _highlightTracker;

        private List<EventTracker<Button>> _trackers;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            _interactionTracker = new PointerSelectionTracker<Button>(this);
            _highlightTracker = new PointerPresenceTracker<Button>(this);

            _trackers = new List<EventTracker<Button>> {_interactionTracker, _highlightTracker};

            _trackers.ForEach(t => t.Initialize());

            _interactable
                .Subscribe(v =>
                {
                    PeerButton.interactable = v;
                    _trackers.ForEach(t => t.Active = v);
                })
                .AddTo(this);

            _background
                .Subscribe(v => v.Update(PeerImage, DefaultBackground))
                .AddTo(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _trackers?.ForEach(t => t.Dispose());
            _trackers = null;
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            PeerButton.interactable = _interactable.Value;
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            Background.Update(PeerImage, DefaultBackground);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Button) component;

            Interactable = source.Interactable;

            Background = new ImageAndColor(source.Background);
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public new static Button CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Button");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Button>();
        }
    }
}