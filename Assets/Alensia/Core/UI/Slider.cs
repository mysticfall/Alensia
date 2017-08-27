using System.Collections.Generic;
using Alensia.Core.UI.Event;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UESlider = UnityEngine.UI.Slider;

namespace Alensia.Core.UI
{
    [RequireComponent(typeof(UESlider))]
    public class Slider : UIComponent, IInputComponent<float>
    {
        public bool Interactable
        {
            get { return _interactable.Value; }
            set { _interactable.Value = value; }
        }

        public bool Interacting => _interactionTracker != null && _interactionTracker.State;

        public bool Highlighted => _highlightTracker != null && _highlightTracker.State;

        public float Value
        {
            get { return _value.Value; }
            set { _value.Value = value; }
        }

        public float MinValue
        {
            get { return _minValue.Value; }
            set { _minValue.Value = value; }
        }

        public float MaxValue
        {
            get { return _maxValue.Value; }
            set { _maxValue.Value = value; }
        }

        public IObservable<float> OnValueChange => _value;

        public IObservable<bool> OnInteractableStateChange => _interactable;

        public IObservable<bool> OnInteractingStateChange => _interactionTracker?.OnStateChange;

        public IObservable<bool> OnHighlightedStateChange => _highlightTracker?.OnStateChange;

        protected UESlider PeerSlider =>
            _peerSlider ?? (_peerSlider = GetComponentInChildren<UESlider>());

        protected Image PeerBackground =>
            _peerBackground ?? (_peerBackground = Transform.Find("Background").GetComponent<Image>());

        protected Transform PeerFillArea =>
            _peerFillArea ?? (_peerFillArea = Transform.Find("Fill Area"));

        protected Transform PeerHandleSlideArea =>
            _peerHandleSlideArea ?? (_peerHandleSlideArea = Transform.Find("Handle Slide Area"));

        protected Image PeerHandle =>
            _peerHandle ?? (_peerHandle = PeerHandleSlideArea.Find("Handle").GetComponent<Image>());

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerSlider != null) peers.Add(PeerSlider);

                if (PeerBackground != null) peers.Add(PeerBackground.gameObject);
                if (PeerFillArea != null) peers.Add(PeerFillArea.gameObject);
                if (PeerHandleSlideArea != null) peers.Add(PeerHandleSlideArea.gameObject);

                return peers;
            }
        }

        [SerializeField] private BoolReactiveProperty _interactable;

        [SerializeField] private FloatReactiveProperty _value;

        [SerializeField] private FloatReactiveProperty _minValue;

        [SerializeField] private FloatReactiveProperty _maxValue;

        [SerializeField, HideInInspector] private UESlider _peerSlider;

        [SerializeField, HideInInspector] private Image _peerBackground;

        [SerializeField, HideInInspector] private Transform _peerFillArea;

        [SerializeField, HideInInspector] private Transform _peerHandleSlideArea;

        [SerializeField, HideInInspector] private Image _peerHandle;

        private EventTracker<Image> _interactionTracker;

        private EventTracker<Image> _highlightTracker;

        private List<EventTracker<Image>> _trackers;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            _interactionTracker = new PointerSelectionTracker<Image>(PeerHandle);
            _highlightTracker = new PointerPresenceTracker<Image>(PeerHandle);

            _trackers = new List<EventTracker<Image>> {_interactionTracker, _highlightTracker};

            _trackers.ForEach(t => t.Initialize());

            _interactable
                .Subscribe(v =>
                {
                    PeerSlider.interactable = v;
                    _trackers.ForEach(t => t.Active = v);
                })
                .AddTo(this);
            
            _minValue.Subscribe(v => PeerSlider.minValue = v).AddTo(this);
            _maxValue.Subscribe(v => PeerSlider.maxValue = v).AddTo(this);
            _value.Subscribe(v => PeerSlider.value = v).AddTo(this);

            PeerSlider
                .OnValueChangedAsObservable()
                .Subscribe(v => Value = v)
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

            PeerSlider.minValue = MinValue;
            PeerSlider.maxValue = MaxValue;
            PeerSlider.value = Value;
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            MinValue = 0;
            MaxValue = 1;
            Value = 0;
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public static Slider CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Slider");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Slider>();
        }
    }
}