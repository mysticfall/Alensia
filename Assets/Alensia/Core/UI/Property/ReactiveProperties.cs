using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Alensia.Core.UI.Property
{
    [Serializable]
    public class SpriteReactiveProperty : ReactiveProperty<Sprite>
    {
        public SpriteReactiveProperty()
        {
        }

        public SpriteReactiveProperty(Sprite sprite) : base(sprite)
        {
        }
    }

    [Serializable]
    public class ImageTypeReactiveProperty : ReactiveProperty<Image.Type>
    {
        public ImageTypeReactiveProperty()
        {
        }

        public ImageTypeReactiveProperty(Image.Type type) : base(type)
        {
        }
    }

    [Serializable]
    public class ColorReactiveProperty : ReactiveProperty<Color>
    {
        public ColorReactiveProperty()
        {
        }

        public ColorReactiveProperty(Color color) : base(color)
        {
        }
    }

    [Serializable]
    public class FontReactiveProperty : ReactiveProperty<Font>
    {
        public FontReactiveProperty()
        {
        }

        public FontReactiveProperty(Font font) : base(font)
        {
        }
    }

    [Serializable]
    public class FontStyleReactiveProperty : ReactiveProperty<FontStyle>
    {
        public FontStyleReactiveProperty()
        {
        }

        public FontStyleReactiveProperty(FontStyle fontStyle) : base(fontStyle)
        {
        }
    }

    [Serializable]
    public class TextAnchorReactiveProperty : ReactiveProperty<TextAnchor>
    {
        public TextAnchorReactiveProperty()
        {
        }

        public TextAnchorReactiveProperty(TextAnchor textAnchor) : base(textAnchor)
        {
        }
    }

    [Serializable]
    public class HorizontalWrapModeReactiveProperty : ReactiveProperty<HorizontalWrapMode>
    {
        public HorizontalWrapModeReactiveProperty()
        {
        }

        public HorizontalWrapModeReactiveProperty(HorizontalWrapMode mode) : base(mode)
        {
        }
    }

    [Serializable]
    public class VerticalWrapModeReactiveProperty : ReactiveProperty<VerticalWrapMode>
    {
        public VerticalWrapModeReactiveProperty()
        {
        }

        public VerticalWrapModeReactiveProperty(VerticalWrapMode mode) : base(mode)
        {
        }
    }
}