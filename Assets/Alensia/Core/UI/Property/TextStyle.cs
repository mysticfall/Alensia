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
        public Font Font
        {
            get { return _font.Value; }
            set { _font.Value = value; }
        }

        public int FontSize
        {
            get { return _fontSize.Value; }
            set { _fontSize.Value = value; }
        }

        public FontStyle FontStyle
        {
            get { return _fontStyle.Value; }
            set { _fontStyle.Value = value; }
        }

        public Color Color
        {
            get { return _color.Value; }
            set { _color.Value = value; }
        }

        public TextAnchor Alignment
        {
            get { return _alignment.Value; }
            set { _alignment.Value = value; }
        }

        public HorizontalWrapMode HorizontalOverflow
        {
            get { return _horizontalOverflow.Value; }
            set { _horizontalOverflow.Value = value; }
        }

        public VerticalWrapMode VerticalOverflow
        {
            get { return _verticalOverflow.Value; }
            set { _verticalOverflow.Value = value; }
        }

        public float LineSpacing
        {
            get { return _lineSpacing.Value; }
            set { _lineSpacing.Value = value; }
        }

        public UniRx.IObservable<TextStyle> OnChange
        {
            get
            {
                return _font.Select(_ => this)
                    .Merge(_fontSize.Select(_ => this))
                    .Merge(_fontStyle.Select(_ => this))
                    .Merge(_color.Select(_ => this))
                    .Merge(_alignment.Select(_ => this))
                    .Merge(_horizontalOverflow.Select(_ => this))
                    .Merge(_verticalOverflow.Select(_ => this))
                    .Merge(_lineSpacing.Select(_ => this));
            }
        }

        [SerializeField] private FontReactiveProperty _font;

        [SerializeField] private IntReactiveProperty _fontSize;

        [SerializeField] private FontStyleReactiveProperty _fontStyle;

        [SerializeField] private TextAnchorReactiveProperty _alignment;

        [SerializeField] private HorizontalWrapModeReactiveProperty _horizontalOverflow;

        [SerializeField] private VerticalWrapModeReactiveProperty _verticalOverflow;

        [SerializeField] private FloatReactiveProperty _lineSpacing;

        [SerializeField] private ColorReactiveProperty _color;

        public TextStyle()
        {
            _font = new FontReactiveProperty();
            _fontSize = new IntReactiveProperty();
            _fontStyle = new FontStyleReactiveProperty();
            _color = new ColorReactiveProperty();
            _alignment = new TextAnchorReactiveProperty();
            _horizontalOverflow = new HorizontalWrapModeReactiveProperty();
            _verticalOverflow = new VerticalWrapModeReactiveProperty();
            _lineSpacing = new FloatReactiveProperty();
        }

        public void Load(Text value)
        {
            Assert.IsNotNull(value);

            _font.Value = value.font;
            _fontSize.Value = value.fontSize;
            _fontStyle.Value = value.fontStyle;
            _color.Value = value.color;
            _alignment.Value = value.alignment;
            _horizontalOverflow.Value = value.horizontalOverflow;
            _verticalOverflow.Value = value.verticalOverflow;
            _lineSpacing.Value = value.lineSpacing;
        }

        public void Update(Text value)
        {
            Assert.IsNotNull(value);

            value.font = _font.Value;
            value.fontSize = _fontSize.Value;
            value.fontStyle = _fontStyle.Value;
            value.color = _color.Value;
            value.alignment = _alignment.Value;
            value.horizontalOverflow = _horizontalOverflow.Value;
            value.verticalOverflow = _verticalOverflow.Value;
            value.lineSpacing = _lineSpacing.Value;
        }
    }
}