using System.Collections.Generic;
using Alensia.Core.UI.Property;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UESlider = UnityEngine.UI.Slider;

namespace Alensia.Core.UI
{
    public class Slider : InteractableComponent<UESlider, Image>, IInputComponent<float>
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

        public ImageAndColorSet Background
        {
            get { return _background.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _background.Value = value;
            }
        }

        public ImageAndColorSet FillImage
        {
            get { return _fillImage.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _fillImage.Value = value;
            }
        }

        public ImageAndColorSet HandleImage
        {
            get { return _handleImage.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _handleImage.Value = value;
            }
        }

        public IObservable<float> OnValueChange => _value;

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = DefaultBackgroundSet;

                return value?.ValueFor(this)?.Merge(base.DefaultBackground) ?? base.DefaultBackground;
            }
        }

        protected ImageAndColorSet DefaultBackgroundSet => Style?.ImageAndColorSets?["Slider.Background"];

        protected ImageAndColor DefaultFillImage => DefaultFillImageSet?.ValueFor(this);

        protected ImageAndColorSet DefaultFillImageSet => Style?.ImageAndColorSets?["Slider.FillImage"];

        protected ImageAndColor DefaultHandleImage => DefaultHandleImageSet?.ValueFor(this);

        protected ImageAndColorSet DefaultHandleImageSet => Style?.ImageAndColorSets?["Slider.HandleImage"];

        protected UESlider PeerSlider =>
            _peerSlider ?? (_peerSlider = GetComponentInChildren<UESlider>());

        protected Image PeerBackground =>
            _peerBackground ?? (_peerBackground = Transform.Find("Background").GetComponent<Image>());

        protected Transform PeerFillArea =>
            _peerFillArea ?? (_peerFillArea = Transform.Find("Fill Area"));

        protected Image PeerFill =>
            _peerFill ?? (_peerFill = PeerFillArea.Find("Fill").GetComponent<Image>());

        protected Transform PeerHandleSlideArea =>
            _peerHandleSlideArea ?? (_peerHandleSlideArea = Transform.Find("Handle Slide Area"));

        protected Image PeerHandle =>
            _peerHandle ?? (_peerHandle = PeerHandleSlideArea.Find("Handle").GetComponent<Image>());

        protected override UESlider PeerSelectable => PeerSlider;

        protected override Image PeerHotspot => PeerHandle;

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

        [SerializeField] private FloatReactiveProperty _value;

        [SerializeField] private FloatReactiveProperty _minValue;

        [SerializeField] private FloatReactiveProperty _maxValue;

        [SerializeField] private ImageAndColorSetReactiveProperty _background;

        [SerializeField] private ImageAndColorSetReactiveProperty _fillImage;

        [SerializeField] private ImageAndColorSetReactiveProperty _handleImage;

        [SerializeField, HideInInspector] private UESlider _peerSlider;

        [SerializeField, HideInInspector] private Image _peerBackground;

        [SerializeField, HideInInspector] private Image _peerFill;

        [SerializeField, HideInInspector] private Image _peerHandle;

        private Transform _peerFillArea;

        private Transform _peerHandleSlideArea;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            PeerSlider
                .OnValueChangedAsObservable()
                .Subscribe(v => Value = v)
                .AddTo(this);

            _minValue
                .Subscribe(v => PeerSlider.minValue = v)
                .AddTo(this);
            _maxValue
                .Subscribe(v => PeerSlider.maxValue = v)
                .AddTo(this);
            _value
                .Subscribe(v => PeerSlider.value = v)
                .AddTo(this);

            _background
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerBackground, DefaultBackground))
                .AddTo(this);
            _fillImage
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerFill, DefaultFillImage))
                .AddTo(this);
            _handleImage
                .Select(v => v.ValueFor(this))
                .Subscribe(v => v.Update(PeerHandle, DefaultHandleImage))
                .AddTo(this);
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            PeerSlider.minValue = MinValue;
            PeerSlider.maxValue = MaxValue;
            PeerSlider.value = Value;
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            Background.ValueFor(this).Update(PeerBackground, DefaultBackground);
            FillImage.ValueFor(this).Update(PeerFill, DefaultFillImage);
            HandleImage.ValueFor(this).Update(PeerHandle, DefaultHandleImage);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Slider) component;

            MinValue = 0;
            MaxValue = 1;
            Value = 0;

            Background = new ImageAndColorSet(source.Background);
            FillImage = new ImageAndColorSet(source.FillImage);
            HandleImage = new ImageAndColorSet(source.HandleImage);
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