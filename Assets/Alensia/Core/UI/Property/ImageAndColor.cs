using System;
using ModestTree;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Alensia.Core.UI.Property
{
    [Serializable]
    public class ImageAndColor : ICompositeProperty<ImageAndColor, Image>
    {
        public Color Color
        {
            get { return _color.Value; }
            set { _color.Value = value; }
        }

        public Sprite Image
        {
            get { return _image.Value; }
            set { _image.Value = value; }
        }

        public Image.Type Type
        {
            get { return _type.Value; }
            set { _type.Value = value; }
        }

        public UniRx.IObservable<ImageAndColor> OnChange
        {
            get
            {
                return _color.Select(_ => this)
                    .Merge(_image.Select(_ => this))
                    .Merge(_type.Select(_ => this));
            }
        }

        [SerializeField] private ColorReactiveProperty _color;

        [SerializeField] private SpriteReactiveProperty _image;

        [SerializeField] private ImageTypeReactiveProperty _type;

        public ImageAndColor()
        {
            _color = new ColorReactiveProperty();
            _image = new SpriteReactiveProperty();
            _type = new ImageTypeReactiveProperty();
        }

        public void Load(Image value)
        {
            Assert.IsNotNull(value);

            _color.Value = value.color;
            _image.Value = value.sprite;
            _type.Value = value.type;
        }

        public void Update(Image value)
        {
            Assert.IsNotNull(value);

            value.color = _color.Value;
            value.sprite = _image.Value;
            value.type = _type.Value;
        }
    }
}