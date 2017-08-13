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
}