using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Alensia.Core.UI.Property
{
    public abstract class UnsettableProperty<T>
    {
        public T Value => _value;

        public bool HasValue => _hasValue;

        [SerializeField] private bool _hasValue;

        [SerializeField] private T _value;

        protected UnsettableProperty()
        {
        }

        protected UnsettableProperty(T value)
        {
            _hasValue = true;

            if (_hasValue)
            {
                _value = value;
            }
        }

        public T OrDefault(T defaultValue) => HasValue ? Value : defaultValue;

        public T OrDefault(UnsettableProperty<T> defaultValue)
        {
            return HasValue ? Value : (defaultValue == null ? default(T) : defaultValue.Value);
        }
    }

    [Serializable]
    public class UnsettableInt : UnsettableProperty<int>
    {
        public UnsettableInt()
        {
        }

        public UnsettableInt(int value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableFloat : UnsettableProperty<float>
    {
        public UnsettableFloat()
        {
        }

        public UnsettableFloat(float value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableBool : UnsettableProperty<bool>
    {
        public UnsettableBool()
        {
        }

        public UnsettableBool(bool value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableColor : UnsettableProperty<Color>
    {
        public UnsettableColor()
        {
        }

        public UnsettableColor(Color value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableSprite : UnsettableProperty<Sprite>
    {
        public UnsettableSprite()
        {
        }

        public UnsettableSprite(Sprite value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableImageType : UnsettableProperty<Image.Type>
    {
        public UnsettableImageType()
        {
        }

        public UnsettableImageType(Image.Type value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableFont : UnsettableProperty<Font>
    {
        public UnsettableFont()
        {
        }

        public UnsettableFont(Font value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableFontStyle : UnsettableProperty<FontStyle>
    {
        public UnsettableFontStyle()
        {
        }

        public UnsettableFontStyle(FontStyle value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableTextAnchor : UnsettableProperty<TextAnchor>
    {
        public UnsettableTextAnchor()
        {
        }

        public UnsettableTextAnchor(TextAnchor value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableHorizontalWrapMode : UnsettableProperty<HorizontalWrapMode>
    {
        public UnsettableHorizontalWrapMode()
        {
        }

        public UnsettableHorizontalWrapMode(HorizontalWrapMode value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableVerticalWrapMode : UnsettableProperty<VerticalWrapMode>
    {
        public UnsettableVerticalWrapMode()
        {
        }

        public UnsettableVerticalWrapMode(VerticalWrapMode value) : base(value)
        {
        }
    }

    [Serializable]
    public class UnsettableColorReactiveProperty : ReactiveProperty<UnsettableColor>
    {
        public UnsettableColorReactiveProperty()
        {
        }

        public UnsettableColorReactiveProperty(UnsettableColor initialValue) : base(initialValue)
        {
        }

        public UnsettableColorReactiveProperty(UniRx.IObservable<UnsettableColor> source) : base(source)
        {
        }

        public UnsettableColorReactiveProperty(
            UniRx.IObservable<UnsettableColor> source,
            UnsettableColor initialValue) : base(source, initialValue)
        {
        }
    }
}