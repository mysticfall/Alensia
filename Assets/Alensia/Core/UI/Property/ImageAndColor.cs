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
        public Color Color => _color;

        public Sprite Image => _image;

        public Image.Type Type => _type;

        [SerializeField] private Color _color;

        [SerializeField] private Sprite _image;

        [SerializeField] private Image.Type _type;

        public ImageAndColor(Color color, Sprite image, Image.Type type)
        {
            _color = color;
            _image = image;
            _type = type;
        }

        public void Load(Image value)
        {
            Assert.IsNotNull(value);

            _color = value.color;
            _image = value.sprite;
            _type = value.type;
        }

        public void Update(Image value)
        {
            Assert.IsNotNull(value);

            value.color = _color;
            value.sprite = _image;
            value.type = _type;
        }

        public ImageAndColor WithColor(Color color) => new ImageAndColor(color, Image, Type);

        public ImageAndColor WithImage(Sprite image) => new ImageAndColor(Color, image, Type);

        public ImageAndColor WithType(Image.Type type) => new ImageAndColor(Color, Image, type);

        protected bool Equals(ImageAndColor other)
        {
            return _color.Equals(other._color) && Equals(_image, other._image) && _type == other._type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == typeof(ImageAndColor) && Equals((ImageAndColor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Color.GetHashCode();

                hashCode = (hashCode * 397) ^ (Image != null ? Image.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Type;

                return hashCode;
            }
        }
    }

    [Serializable]
    public class ImageAndColorReactiveProperty : ReactiveProperty<ImageAndColor>
    {
        public ImageAndColorReactiveProperty()
        {
        }

        public ImageAndColorReactiveProperty(
            ImageAndColor initialValue) : base(initialValue)
        {
        }

        public ImageAndColorReactiveProperty(UniRx.IObservable<ImageAndColor> source) : base(source)
        {
        }

        public ImageAndColorReactiveProperty(UniRx.IObservable<ImageAndColor> source,
            ImageAndColor initialValue) : base(source, initialValue)
        {
        }
    }
}