using System;
using Alensia.Core.Common;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI.Property
{
    [Serializable]
    public class ColorSet : ITransitionalProperty<UnsettableColor>, IMergeableProperty<ColorSet>,
        IEditorSettings
    {
        public UnsettableColor Normal => _normal;

        public UnsettableColor Disabled => _disabled;

        public UnsettableColor Highlighted => _highlighted;

        public UnsettableColor Active => _active;

        [SerializeField] private UnsettableColor _normal;

        [SerializeField] private UnsettableColor _disabled;

        [SerializeField] private UnsettableColor _highlighted;

        [SerializeField] private UnsettableColor _active;

        public ColorSet() : this(null, null, null, null)
        {
        }

        public ColorSet(
            UnsettableColor normal,
            UnsettableColor disabled,
            UnsettableColor highlighted,
            UnsettableColor active)
        {
            _normal = normal ?? new UnsettableColor();
            _disabled = disabled ?? new UnsettableColor();
            _highlighted = highlighted ?? new UnsettableColor();
            _active = active ?? new UnsettableColor();
        }

        public ColorSet(ColorSet source)
        {
            Assert.IsNotNull(source);

            _normal = source.Normal;
            _disabled = source.Disabled;
            _highlighted = source.Highlighted;
            _active = source.Active;
        }

        public ColorSet Merge(ColorSet other)
        {
            return other == null
                ? this
                : new ColorSet(
                    Normal ?? other.Normal,
                    Disabled ?? other.Disabled,
                    Highlighted ?? other.Highlighted,
                    Active ?? other.Active);
        }

        public UnsettableColor ValueFor(IInteractableComponent component) =>
            ValueFor(!component.Interactable, component.Highlighted, component.Interacting);

        public UnsettableColor ValueFor(bool disabled, bool highlighted, bool active)
        {
            if (disabled) return Disabled ?? Normal;
            if (active) return Active ?? Highlighted ?? Normal;
            if (highlighted) return Highlighted ?? Normal;

            return Normal;
        }

        public ColorSet WithNormalValue(UnsettableColor value) => new ColorSet(value, Disabled, Highlighted, Active);

        public ColorSet WithDisabledValue(UnsettableColor value) => new ColorSet(Normal, value, Highlighted, Active);

        public ColorSet WithHighlightedValue(UnsettableColor value) => new ColorSet(Normal, Disabled, value, Active);

        public ColorSet WithActiveValue(UnsettableColor value) => new ColorSet(Normal, Disabled, Highlighted, value);

        protected bool Equals(ColorSet other)
        {
            return Equals(_normal, other._normal) && Equals(_disabled, other._disabled) &&
                   Equals(_highlighted, other._highlighted) && Equals(_active, other._active);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((ColorSet) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Normal != null ? Normal.GetHashCode() : 0;

                hashCode = (hashCode * 397) ^ (Disabled != null ? Disabled.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Highlighted != null ? Highlighted.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Active != null ? Active.GetHashCode() : 0);

                return hashCode;
            }
        }
    }

    [Serializable]
    public class ColorSetReactiveProperty : ReactiveProperty<ColorSet>
    {
        public ColorSetReactiveProperty()
        {
        }

        public ColorSetReactiveProperty(ColorSet initialValue) : base(initialValue)
        {
        }

        public ColorSetReactiveProperty(UniRx.IObservable<ColorSet> source) : base(source)
        {
        }

        public ColorSetReactiveProperty(UniRx.IObservable<ColorSet> source, ColorSet initialValue) : base(source, initialValue)
        {
        }
    }
}