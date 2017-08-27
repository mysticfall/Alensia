using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI.Property
{
    [Serializable]
    public class TextStyle : ICompositeProperty<TextStyle, Text>, IMergeableProperty<TextStyle>
    {
        public UnsettableFont Font => _font;

        public UnsettableInt FontSize => _fontSize;

        public UnsettableFontStyle FontStyle => _fontStyle;

        public UnsettableColor Color => _color;

        public UnsettableTextAnchor Alignment => _alignment;

        public UnsettableHorizontalWrapMode HorizontalOverflow => _horizontalOverflow;

        public UnsettableVerticalWrapMode VerticalOverflow => _verticalOverflow;

        public UnsettableFloat LineSpacing => _lineSpacing;

        [SerializeField] private UnsettableFont _font;

        [SerializeField] private UnsettableInt _fontSize;

        [SerializeField] private UnsettableFontStyle _fontStyle;

        [SerializeField] private UnsettableTextAnchor _alignment;

        [SerializeField] private UnsettableHorizontalWrapMode _horizontalOverflow;

        [SerializeField] private UnsettableVerticalWrapMode _verticalOverflow;

        [SerializeField] private UnsettableFloat _lineSpacing;

        [SerializeField] private UnsettableColor _color;

        public TextStyle()
        {
            _fontSize = new UnsettableInt();
            _fontStyle = new UnsettableFontStyle();
            _alignment = new UnsettableTextAnchor();
            _horizontalOverflow = new UnsettableHorizontalWrapMode();
            _verticalOverflow = new UnsettableVerticalWrapMode();
            _lineSpacing = new UnsettableFloat();
            _color = new UnsettableColor();
        }

        public TextStyle(
            UnsettableFont font,
            UnsettableInt fontSize,
            UnsettableFontStyle fontStyle,
            UnsettableTextAnchor alignment,
            UnsettableHorizontalWrapMode horizontalOverflow,
            UnsettableVerticalWrapMode verticalOverflow,
            UnsettableFloat lineSpacing,
            UnsettableColor color)
        {
            _font = font ?? new UnsettableFont();
            _fontSize = fontSize ?? new UnsettableInt();
            _fontStyle = fontStyle ?? new UnsettableFontStyle();
            _alignment = alignment ?? new UnsettableTextAnchor();
            _horizontalOverflow = horizontalOverflow ?? new UnsettableHorizontalWrapMode();
            _verticalOverflow = verticalOverflow ?? new UnsettableVerticalWrapMode();
            _lineSpacing = lineSpacing ?? new UnsettableFloat();
            _color = color ?? new UnsettableColor();
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

        public void Update(Text source) => Update(source, null);

        public void Update(Text source, TextStyle defaultValue)
        {
            Assert.IsNotNull(source);

            source.font = _font.OrDefault(defaultValue?.Font);
            source.fontSize = _fontSize.OrDefault(defaultValue?.FontSize);
            source.fontStyle = _fontStyle.OrDefault(defaultValue?.FontStyle);
            source.color = _color.OrDefault(defaultValue?.Color);
            source.alignment = _alignment.OrDefault(defaultValue?.Alignment);
            source.horizontalOverflow = _horizontalOverflow.OrDefault(defaultValue?.HorizontalOverflow);
            source.verticalOverflow = _verticalOverflow.OrDefault(defaultValue?.VerticalOverflow);
            source.lineSpacing = _lineSpacing.OrDefault(defaultValue?.LineSpacing);
        }

        public TextStyle Merge(TextStyle other)
        {
            return other == null
                ? this
                : new TextStyle(
                    Font.HasValue ? Font : other.Font,
                    FontSize.HasValue ? FontSize : other.FontSize,
                    FontStyle.HasValue ? FontStyle : other.FontStyle,
                    Alignment.HasValue ? Alignment : other.Alignment,
                    HorizontalOverflow.HasValue ? HorizontalOverflow : other.HorizontalOverflow,
                    VerticalOverflow.HasValue ? VerticalOverflow : other.VerticalOverflow,
                    LineSpacing.HasValue ? LineSpacing : other.LineSpacing,
                    Color.HasValue ? Color : other.Color);
        }

        public TextStyle WithFont(UnsettableFont font) =>
            new TextStyle(
                font,
                FontSize,
                FontStyle,
                Alignment,
                HorizontalOverflow,
                VerticalOverflow,
                LineSpacing,
                Color);

        public TextStyle WithFontSize(UnsettableInt fontSize) =>
            new TextStyle(
                Font,
                fontSize,
                FontStyle,
                Alignment,
                HorizontalOverflow,
                VerticalOverflow,
                LineSpacing,
                Color);

        public TextStyle WithFontStyle(UnsettableFontStyle fontStyle) =>
            new TextStyle(
                Font,
                FontSize,
                fontStyle,
                Alignment,
                HorizontalOverflow,
                VerticalOverflow,
                LineSpacing,
                Color);

        public TextStyle WithAlignment(UnsettableTextAnchor alignment) =>
            new TextStyle(
                Font,
                FontSize,
                FontStyle,
                alignment,
                HorizontalOverflow,
                VerticalOverflow,
                LineSpacing,
                Color);

        public TextStyle WithHorizontalOverflow(UnsettableHorizontalWrapMode overflow) =>
            new TextStyle(
                Font,
                FontSize,
                FontStyle,
                Alignment,
                overflow,
                VerticalOverflow,
                LineSpacing,
                Color);

        public TextStyle WithVerticalOverflow(UnsettableVerticalWrapMode overflow) =>
            new TextStyle(
                Font,
                FontSize,
                FontStyle,
                Alignment,
                HorizontalOverflow,
                overflow,
                LineSpacing,
                Color);

        public TextStyle WithLineSpacing(UnsettableFloat lineSpacing) =>
            new TextStyle(
                Font,
                FontSize,
                FontStyle,
                Alignment,
                HorizontalOverflow,
                VerticalOverflow,
                lineSpacing,
                Color);

        public TextStyle WithColor(UnsettableColor color) =>
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
                var hashCode = Font != null ? Font.GetHashCode() : 0;

                hashCode = (hashCode * 397) ^ (FontSize != null ? FontSize.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FontStyle != null ? FontStyle.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Alignment != null ? Alignment.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HorizontalOverflow != null ? HorizontalOverflow.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (VerticalOverflow != null ? VerticalOverflow.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LineSpacing != null ? LineSpacing.GetHashCode() : 0);
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