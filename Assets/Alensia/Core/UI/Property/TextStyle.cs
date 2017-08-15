using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI.Property
{
    [Serializable]
    public class TextStyle : ICompositeProperty<TextStyle, Text>
    {
        public Font Font => _font;

        public int FontSize => _fontSize;

        public FontStyle FontStyle => _fontStyle;

        public Color Color => _color;

        public TextAnchor Alignment => _alignment;

        public HorizontalWrapMode HorizontalOverflow => _horizontalOverflow;

        public VerticalWrapMode VerticalOverflow => _verticalOverflow;

        public float LineSpacing => _lineSpacing;

        [SerializeField] private Font _font;

        [SerializeField] private int _fontSize;

        [SerializeField] private FontStyle _fontStyle;

        [SerializeField] private TextAnchor _alignment;

        [SerializeField] private HorizontalWrapMode _horizontalOverflow;

        [SerializeField] private VerticalWrapMode _verticalOverflow;

        [SerializeField] private float _lineSpacing;

        [SerializeField] private Color _color;

        public TextStyle()
        {
            _font = new Font("Arial");
            _fontSize = 14;
            _fontStyle = FontStyle.Normal;
            _alignment = TextAnchor.MiddleCenter;
            _horizontalOverflow = HorizontalWrapMode.Wrap;
            _verticalOverflow = VerticalWrapMode.Truncate;
            _lineSpacing = 1f;
            _color = Color.black;
        }

        public TextStyle(
            Font font,
            int fontSize,
            FontStyle fontStyle,
            TextAnchor alignment,
            HorizontalWrapMode horizontalOverflow,
            VerticalWrapMode verticalOverflow,
            float lineSpacing,
            Color color)
        {
            _font = font;
            _fontSize = fontSize;
            _fontStyle = fontStyle;
            _alignment = alignment;
            _horizontalOverflow = horizontalOverflow;
            _verticalOverflow = verticalOverflow;
            _lineSpacing = lineSpacing;
            _color = color;
        }

        public TextStyle(TextStyle source)
        {
            Assert.IsNotNull(source, "source != null");

            _font = source.Font;
            _fontSize = source.FontSize;
            _fontStyle = source.FontStyle;
            _alignment = source.Alignment;
            _horizontalOverflow = source.HorizontalOverflow;
            _verticalOverflow = source.VerticalOverflow;
            _lineSpacing = source.LineSpacing;
            _color = source.Color;
        }

        public void Load(Text value)
        {
            Assert.IsNotNull(value);

            _font = value.font;
            _fontSize = value.fontSize;
            _fontStyle = value.fontStyle;
            _color = value.color;
            _alignment = value.alignment;
            _horizontalOverflow = value.horizontalOverflow;
            _verticalOverflow = value.verticalOverflow;
            _lineSpacing = value.lineSpacing;
        }

        public void Update(Text value)
        {
            Assert.IsNotNull(value);

            value.font = _font;
            value.fontSize = _fontSize;
            value.fontStyle = _fontStyle;
            value.color = _color;
            value.alignment = _alignment;
            value.horizontalOverflow = _horizontalOverflow;
            value.verticalOverflow = _verticalOverflow;
            value.lineSpacing = _lineSpacing;
        }

        public TextStyle WithFont(Font font) =>
            new TextStyle(
                font,
                FontSize,
                FontStyle,
                Alignment,
                HorizontalOverflow,
                VerticalOverflow,
                LineSpacing,
                Color);

        public TextStyle WithFontSize(int fontSize) =>
            new TextStyle(
                Font,
                fontSize,
                FontStyle,
                Alignment,
                HorizontalOverflow,
                VerticalOverflow,
                LineSpacing,
                Color);

        public TextStyle WithFontStyle(FontStyle fontStyle) =>
            new TextStyle(
                Font,
                FontSize,
                fontStyle,
                Alignment,
                HorizontalOverflow,
                VerticalOverflow,
                LineSpacing,
                Color);

        public TextStyle WithAlignment(TextAnchor alignment) =>
            new TextStyle(
                Font,
                FontSize,
                FontStyle,
                alignment,
                HorizontalOverflow,
                VerticalOverflow,
                LineSpacing,
                Color);

        public TextStyle WithHorizontalOverflow(HorizontalWrapMode overflow) =>
            new TextStyle(
                Font,
                FontSize,
                FontStyle,
                Alignment,
                overflow,
                VerticalOverflow,
                LineSpacing,
                Color);

        public TextStyle WithVerticalOverflow(VerticalWrapMode overflow) =>
            new TextStyle(
                Font,
                FontSize,
                FontStyle,
                Alignment,
                HorizontalOverflow,
                overflow,
                LineSpacing,
                Color);

        public TextStyle WithLineSpacing(float lineSpacing) =>
            new TextStyle(
                Font,
                FontSize,
                FontStyle,
                Alignment,
                HorizontalOverflow,
                VerticalOverflow,
                lineSpacing,
                Color);

        public TextStyle WithColor(Color color) =>
            new TextStyle(
                Font,
                FontSize,
                FontStyle,
                Alignment,
                HorizontalOverflow,
                VerticalOverflow,
                LineSpacing,
                color);

        protected bool Equals(TextStyle other)
        {
            return Equals(_font, other._font) &&
                   _fontSize == other._fontSize &&
                   _fontStyle == other._fontStyle &&
                   _alignment == other._alignment &&
                   _horizontalOverflow == other._horizontalOverflow &&
                   _verticalOverflow == other._verticalOverflow &&
                   _lineSpacing.Equals(other._lineSpacing) &&
                   _color.Equals(other._color);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == typeof(TextStyle) && Equals((TextStyle) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Font != null ? Font.GetHashCode() : 0);

                hashCode = (hashCode * 397) ^ FontSize;
                hashCode = (hashCode * 397) ^ (int) FontStyle;
                hashCode = (hashCode * 397) ^ (int) Alignment;
                hashCode = (hashCode * 397) ^ (int) HorizontalOverflow;
                hashCode = (hashCode * 397) ^ (int) VerticalOverflow;
                hashCode = (hashCode * 397) ^ LineSpacing.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();

                return hashCode;
            }
        }
    }

    [Serializable]
    public class TextStyleReactiveProperty : ReactiveProperty<TextStyle>
    {
        public TextStyleReactiveProperty()
        {
        }

        public TextStyleReactiveProperty(
            TextStyle initialValue) : base(initialValue)
        {
        }

        public TextStyleReactiveProperty(UniRx.IObservable<TextStyle> source) : base(source)
        {
        }

        public TextStyleReactiveProperty(UniRx.IObservable<TextStyle> source,
            TextStyle initialValue) : base(source, initialValue)
        {
        }
    }
}