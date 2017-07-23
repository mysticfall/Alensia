using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UESlider = UnityEngine.UI.Slider;

namespace Alensia.Core.UI
{
    [RequireComponent(typeof(UESlider))]
    public class Slider : UIComponent, IInputComponent<float>
    {
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

        public IObservable<float> OnValueChange => PeerSlider.onValueChanged.AsObservable();

        protected UESlider PeerSlider => _peerSlider;

        protected override IList<Component> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerSlider != null) peers.Add(PeerSlider);

                return peers;
            }
        }

        [SerializeField] private FloatReactiveProperty _value;

        [SerializeField] private FloatReactiveProperty _minValue;

        [SerializeField] private FloatReactiveProperty _maxValue;

        [SerializeField, HideInInspector] private UESlider _peerSlider;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            _minValue.Subscribe(v => PeerSlider.minValue = v).AddTo(this);
            _maxValue.Subscribe(v => PeerSlider.maxValue = v).AddTo(this);
            _value.Subscribe(v => PeerSlider.value = v).AddTo(this);
        }

        protected override void InitializePeers()
        {
            base.InitializePeers();

            _peerSlider = GetComponentInChildren<UESlider>();
        }

        protected override void ValidateProperties()
        {
            base.ValidateProperties();

            PeerSlider.minValue = MinValue;
            PeerSlider.maxValue = MaxValue;
            PeerSlider.value = Value;
        }

        protected override void Reset()
        {
            base.Reset();

            MinValue = 0;
            MaxValue = 1;
            Value = 0;
        }

        public static Slider CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Slider");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Slider>();
        }
    }
}