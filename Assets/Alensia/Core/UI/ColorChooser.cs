using System;
using System.Collections.Generic;
using Alensia.Core.Common;
using Alensia.Core.UI.Property;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UECursor = UnityEngine.Cursor;

namespace Alensia.Core.UI
{
    public class ColorChooser : Panel, IInputComponent<Color>
    {
        public Color Value
        {
            get { return _value.Value; }
            set { _value.Value = value; }
        }

        public IObservable<Color> OnValueChange => _value;

        public InputText RedInput => _redInput ?? (_redInput = FindPeer<InputText>("InputRed"));

        public InputText GreenInput => _greenInput ?? (_greenInput = FindPeer<InputText>("InputGreen"));

        public InputText BlueInput => _blueInput ?? (_blueInput = FindPeer<InputText>("InputBlue"));

        public InputText HexInput => _hexInput ?? (_hexInput = FindPeer<InputText>("InputHex"));

        public Slider RedSlider => _redSlider ?? (_redSlider = FindPeer<Slider>("SliderRed"));

        public Slider GreenSlider => _greenSlider ?? (_greenSlider = FindPeer<Slider>("SliderGreen"));

        public Slider BlueSlider => _blueSlider ?? (_blueSlider = FindPeer<Slider>("SliderBlue"));

        protected float CanvasHue
        {
            get
            {
                var bounds = HueCanvasTransform.rect;
                var pos = HuePointerTransform.anchoredPosition.y;

                return 1 - pos / bounds.height;
            }
        }

        protected Color CanvasColor
        {
            get
            {
                var bounds = ColorCanvasTransform.rect;
                var pos = ColorPointerTransform.anchoredPosition;

                var sv = new Vector2(pos.x / bounds.width, pos.y / bounds.height);

                return Color.HSVToRGB(CanvasHue, sv.x, sv.y);
            }
            set
            {
                float h, s, v;

                Color.RGBToHSV(value, out h, out s, out v);

                var colorBounds = ColorCanvasTransform.rect;
                var colorPos = new Vector2(s * colorBounds.width, v * colorBounds.height);

                if (colorPos.x >= 0 && colorPos.y >= 0)
                {
                    ColorPointerTransform.anchoredPosition = colorPos;
                }

                var hueX = HuePointerTransform.anchoredPosition.x;
                var hueY = (1 - h) * HueCanvasTransform.rect.height;

                if (hueX >= 0 && hueY >= 0)
                {
                    HuePointerTransform.anchoredPosition = new Vector2(hueX, hueY);
                }

                PaintColorCanvas(h);
            }
        }

        protected override ImageAndColor DefaultBackground
        {
            get
            {
                var value = Style?.ImagesAndColors?["ColorChooser.Background"];

                return value == null ? base.DefaultBackground : value.Merge(base.DefaultBackground);
            }
        }

        protected RawImage ColorCanvas => _colorCanvas ?? (_colorCanvas = FindPeer<RawImage>("ColorCanvas"));

        protected RawImage HueCanvas => _hueCanvas ?? (_hueCanvas = FindPeer<RawImage>("HueCanvas"));

        protected Image SelectedCanvas => _selectedCanvas ?? (_selectedCanvas = FindPeer<Image>("SelectedColor"));

        protected Image ColorPointer => _colorPointer ?? (_colorPointer = ColorCanvas.FindComponent<Image>("Pointer"));

        protected Image HuePointer => _huePointer ?? (_huePointer = HueCanvas.FindComponent<Image>("Pointer"));

        protected Label RedLabel => _redLabel ?? (_redLabel = FindPeer<Label>("LabelRed"));

        protected Label GreenLabel => _greenLabel ?? (_greenLabel = FindPeer<Label>("LabelGreen"));

        protected Label BlueLabel => _blueLabel ?? (_blueLabel = FindPeer<Label>("LabelBlue"));

        protected RectTransform ColorCanvasTransform =>
            _colorCanvasTransform ?? (_colorCanvasTransform = ColorCanvas.GetComponent<RectTransform>());

        protected RectTransform ColorPointerTransform =>
            _colorPointerTransform ?? (_colorPointerTransform = ColorPointer.GetComponent<RectTransform>());

        protected RectTransform HueCanvasTransform =>
            _hueCanvasTransform ?? (_hueCanvasTransform = HueCanvas.GetComponent<RectTransform>());

        protected RectTransform HuePointerTransform =>
            _huePointerTransform ?? (_huePointerTransform = HuePointer.GetComponent<RectTransform>());

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (ColorCanvas != null) peers.Add(ColorCanvas.gameObject);
                if (HueCanvas != null) peers.Add(HueCanvas.gameObject);
                if (SelectedCanvas != null) peers.Add(SelectedCanvas.gameObject);

                if (RedLabel != null) peers.Add(RedLabel.gameObject);
                if (GreenLabel != null) peers.Add(GreenLabel.gameObject);
                if (BlueLabel != null) peers.Add(BlueLabel.gameObject);

                if (RedInput != null) peers.Add(RedInput.gameObject);
                if (GreenInput != null) peers.Add(GreenInput.gameObject);
                if (BlueInput != null) peers.Add(BlueInput.gameObject);
                if (HexInput != null) peers.Add(HexInput.gameObject);

                if (RedSlider != null) peers.Add(RedSlider.gameObject);
                if (GreenSlider != null) peers.Add(GreenSlider.gameObject);
                if (BlueSlider != null) peers.Add(BlueSlider.gameObject);

                return peers;
            }
        }

        private const int ColorResolution = 100;

        private const int HueSteps = 360;

        [SerializeField] private ColorReactiveProperty _value;

        [SerializeField, HideInInspector] private InputText _redInput;

        [SerializeField, HideInInspector] private InputText _greenInput;

        [SerializeField, HideInInspector] private InputText _blueInput;

        [SerializeField, HideInInspector] private InputText _hexInput;

        [SerializeField, HideInInspector] private Slider _redSlider;

        [SerializeField, HideInInspector] private Slider _greenSlider;

        [SerializeField, HideInInspector] private Slider _blueSlider;

        [SerializeField, HideInInspector] private RawImage _colorCanvas;

        [SerializeField, HideInInspector] private RawImage _hueCanvas;

        [SerializeField, HideInInspector] private Image _selectedCanvas;

        [SerializeField, HideInInspector] private Image _colorPointer;

        [SerializeField, HideInInspector] private Image _huePointer;

        [SerializeField, HideInInspector] private Label _redLabel;

        [SerializeField, HideInInspector] private Label _greenLabel;

        [SerializeField, HideInInspector] private Label _blueLabel;

        [NonSerialized] private RectTransform _colorCanvasTransform;

        [NonSerialized] private RectTransform _colorPointerTransform;

        [NonSerialized] private RectTransform _hueCanvasTransform;

        [NonSerialized] private RectTransform _huePointerTransform;

        private float RedInputValue => StringToColorValue(RedInput.Value);

        private float GreenInputValue => StringToColorValue(GreenInput.Value);

        private float BlueInputValue => StringToColorValue(BlueInput.Value);

        private bool _changingColor;

        private bool _changingHue;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            PaintHueCanvas();
            PaintColorCanvas(CanvasHue);

            var onRedChange = OnValueChange.Select(v => v.r);
            var onGreenChange = OnValueChange.Select(v => v.g);
            var onBlueChange = OnValueChange.Select(v => v.b);

            OnValueChange
                .Where(_ => !_changingColor && !_changingHue)
                .Subscribe(v => CanvasColor = v)
                .AddTo(this);
            OnValueChange
                .Subscribe(v => SelectedCanvas.color = v)
                .AddTo(this);
            OnValueChange
                .Select(ColorUtility.ToHtmlStringRGB)
                .Subscribe(v => HexInput.Value = "#" + v)
                .AddTo(this);

            onRedChange
                .Select(ColorValueToString)
                .Subscribe(v => RedInput.Value = v)
                .AddTo(this);
            onGreenChange
                .Select(ColorValueToString)
                .Subscribe(v => GreenInput.Value = v)
                .AddTo(this);
            onBlueChange
                .Select(ColorValueToString)
                .Subscribe(v => BlueInput.Value = v)
                .AddTo(this);

            onRedChange
                .Subscribe(v => RedSlider.Value = v)
                .AddTo(this);
            onGreenChange
                .Subscribe(v => GreenSlider.Value = v)
                .AddTo(this);
            onBlueChange
                .Subscribe(v => BlueSlider.Value = v)
                .AddTo(this);
        }

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            ColorCanvas
                .OnDragAsObservable()
                .Merge(ColorCanvas.OnPointerClickAsObservable())
                .Subscribe(OnColorCanvasDrag)
                .AddTo(this);
            HueCanvas
                .OnDragAsObservable()
                .Merge(HueCanvas.OnPointerClickAsObservable())
                .Subscribe(OnHueCanvasDrag)
                .AddTo(this);

            Func<List<float>, bool> validate = v => v[0] >= 0 && v[1] >= 0 && v[2] >= 0;

            var onRedChange = RedInput.OnEdit
                .Select(StringToColorValue)
                .Select(v => new List<float> {v, GreenInputValue, BlueInputValue})
                .Where(validate);
            var onGreenChange = GreenInput.OnEdit
                .Select(StringToColorValue)
                .Select(v => new List<float> {RedInputValue, v, BlueInputValue})
                .Where(validate);
            var onBlueChange = BlueInput.OnEdit
                .Select(StringToColorValue)
                .Select(v => new List<float> {RedInputValue, GreenInputValue, v})
                .Where(validate);

            var onInputChange = onRedChange.Merge(onGreenChange, onBlueChange);

            // ReSharper disable once PossibleInvalidOperationException
            var onHexChange = HexInput.OnEdit
                .Select(HexToColor)
                .Where(v => v.HasValue)
                .Select(v => v.Value);

            var onSliderChange = Observable
                .CombineLatest(
                    RedSlider.OnValueChange,
                    GreenSlider.OnValueChange,
                    BlueSlider.OnValueChange);

            onInputChange
                .Cast<List<float>, IList<float>>()
                .Merge(onSliderChange)
                .Select(v => new Color(v[0], v[1], v[2]))
                .Merge(onHexChange)
                .Subscribe(v => Value = v)
                .AddTo(this);
        }

        protected void PaintColorCanvas(float hue = 0)
        {
            if (ColorCanvas.texture != null)
            {
                DestroyImmediate(ColorCanvas.texture);
            }

            var texture = new Texture2D(ColorResolution, ColorResolution)
            {
                hideFlags = HideFlags.DontSave
            };

            for (var s = 0; s < ColorResolution; s++)
            {
                var colors = new Color32[ColorResolution];

                for (var v = 0; v < ColorResolution; v++)
                {
                    colors[v] = Color.HSVToRGB(hue, (float) s / ColorResolution, (float) v / ColorResolution);
                }

                texture.SetPixels32(s, 0, 1, ColorResolution, colors);
            }

            texture.Apply();

            ColorCanvas.texture = texture;
        }

        protected void PaintHueCanvas()
        {
            if (ColorCanvas.texture != null)
            {
                DestroyImmediate(HueCanvas.texture);
            }

            var texture = new Texture2D(1, HueSteps)
            {
                hideFlags = HideFlags.DontSave
            };

            var colors = new Color32[HueSteps];

            for (var i = 0; i < HueSteps; i++)
            {
                colors[HueSteps - 1 - i] = Color.HSVToRGB((float) i / HueSteps, 1, 1);
            }

            texture.SetPixels32(colors);
            texture.Apply();

            _changingHue = true;

            HueCanvas.texture = texture;

            _changingHue = false;
        }

        private void OnColorCanvasDrag(PointerEventData e)
        {
            var bounds = ColorCanvasTransform.rect;

            Vector2 local;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                ColorCanvasTransform, e.position, e.pressEventCamera, out local))
                return;

            var pos = local - bounds.min;

            pos.x = Mathf.Clamp(pos.x, 0f, bounds.width);
            pos.y = Mathf.Clamp(pos.y, 0f, bounds.height);

            ColorPointerTransform.anchoredPosition = pos;

            var ratio = new Vector2(pos.x / bounds.width, pos.y / bounds.height);

            var h = CanvasHue;
            var s = ratio.x;
            var v = ratio.y;

            _changingColor = true;

            Value = Color.HSVToRGB(h, s, v);

            _changingColor = false;
        }

        private void OnHueCanvasDrag(PointerEventData e)
        {
            var bounds = HueCanvasTransform.rect;

            Vector2 local;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                HueCanvasTransform, e.position, e.pressEventCamera, out local))
                return;

            var pos = (local - bounds.min).y;

            pos = Mathf.Clamp(pos, 0f, bounds.height);

            var anchor = HuePointerTransform.anchoredPosition;

            HuePointerTransform.anchoredPosition = new Vector2(anchor.x, pos);

            var hue = 1 - pos / bounds.height;

            PaintColorCanvas(hue);

            Value = CanvasColor;
        }

        private static float StringToColorValue(string value)
        {
            float result;

            var valid = float.TryParse(value, out result);

            return valid ? Mathf.Clamp(result, 0, 255) / 255 : -1;
        }

        private static string ColorValueToString(float value) => ((int) (value * 255)).ToString();

        private static Color? HexToColor(string value)
        {
            Color result;

            var valid = ColorUtility.TryParseHtmlString(value, out result);

            return valid ? new Color?(result) : null;
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (ColorChooser) component;

            Value = source.Value;
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public new static ColorChooser CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/ColorChooser");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<ColorChooser>();
        }
    }
}