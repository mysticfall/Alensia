using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Alensia.Core.UI
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
    public class ImageTypeProperty : ReactiveProperty<Image.Type>
    {
        public ImageTypeProperty()
        {
        }

        public ImageTypeProperty(Image.Type type) : base(type)
        {
        }
    }

    [Serializable]
    public class ColorProperty : ReactiveProperty<Color>
    {
        public ColorProperty()
        {
        }

        public ColorProperty(Color color) : base(color)
        {
        }
    }
}