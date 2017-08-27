using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UEColor = UnityEngine.Color;
using UEImage = UnityEngine.UI.Image;

namespace Alensia.Core.UI.Property
{
    [Serializable]
    public class ImageAndColor : ICompositeProperty<ImageAndColor, UEImage>, IMergeableProperty<ImageAndColor>
    {
        public UnsettableColor Color => _color;

        public UnsettableSprite Image => _image;

        public UnsettableImageType Type => _type;

        [SerializeField] private UnsettableColor _color;

        [SerializeField] private UnsettableSprite _image;

        [SerializeField] private UnsettableImageType _type;

        public ImageAndColor()
        {
            _color = new UnsettableColor();
            _image = new UnsettableSprite();
            _type = new UnsettableImageType();
        }

        public ImageAndColor(
            UnsettableColor color, UnsettableSprite image, UnsettableImageType type)
        {
            _color = color ?? new UnsettableColor();
            _image = image ?? new UnsettableSprite();
            _type = type ?? new UnsettableImageType();
        }

        public ImageAndColor(ImageAndColor source)
        {
            Assert.IsNotNull(source);

            _color = source.Color;
            _image = source.Image;
            _type = source.Type;
        }

        public void Update(UEImage source) => Update(source, null);

        public void Update(UEImage source, ImageAndColor defaultValue)
        {
            Assert.IsNotNull(source, "value != null");

            source.color = _color.OrDefault(defaultValue?.Color);
            source.sprite = _image.OrDefault(defaultValue?.Image);
            source.type = _type.OrDefault(defaultValue?.Type);
        }

        public ImageAndColor Merge(ImageAndColor other)
        {
            return other == null
                ? this
                : new ImageAndColor(
                    Color.HasValue ? Color : other.Color,
                    Image.HasValue ? Image : other.Image,
                    Type.HasValue ? Type : other.Type);
        }

        public ImageAndColor WithColor(UnsettableColor color) => new ImageAndColor(color, Image, Type);

        public ImageAndColor WithImage(UnsettableSprite image) => new ImageAndColor(Color, image, Type);

        public ImageAndColor WithType(UnsettableImageType type) => new ImageAndColor(Color, Image, type);

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
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);

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