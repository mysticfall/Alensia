using System;
using Alensia.Core.Common;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI.Property
{
    [Serializable]
    public class ImageAndColorSet : ITransitionalProperty<ImageAndColor>, IMergeableProperty<ImageAndColorSet>,
        IEditorSettings
    {
        public ImageAndColor Normal => _normal;

        public ImageAndColor Disabled => _disabled;

        public ImageAndColor Highlighted => _highlighted;

        public ImageAndColor Active => _active;

        [SerializeField] private ImageAndColor _normal;

        [SerializeField] private ImageAndColor _disabled;

        [SerializeField] private ImageAndColor _highlighted;

        [SerializeField] private ImageAndColor _active;

        public ImageAndColorSet() : this(null, null, null, null)
        {
        }

        public ImageAndColorSet(
            ImageAndColor normal,
            ImageAndColor disabled,
            ImageAndColor highlighted,
            ImageAndColor active)
        {
            _normal = normal ?? new ImageAndColor();
            _disabled = disabled ?? new ImageAndColor();
            _highlighted = highlighted ?? new ImageAndColor();
            _active = active ?? new ImageAndColor();
        }

        public ImageAndColorSet(ImageAndColorSet source)
        {
            Assert.IsNotNull(source);

            _normal = source.Normal;
            _disabled = source.Disabled;
            _highlighted = source.Highlighted;
            _active = source.Active;
        }

        public ImageAndColorSet Merge(ImageAndColorSet other)
        {
            return other == null
                ? this
                : new ImageAndColorSet(
                    Normal.Merge(other.Normal),
                    Disabled.Merge(other.Disabled),
                    Highlighted.Merge(other.Highlighted),
                    Active.Merge(other.Active));
        }

        public ImageAndColor ValueFor(IInteractableComponent component) => 
            ValueFor(!component.Interactable, component.Highlighted, component.Interacting);

        public ImageAndColor ValueFor(bool disabled, bool highlighted, bool active)
        {
            if (disabled) return Disabled.Merge(Normal);
            if (active) return Active.Merge(Highlighted).Merge(Normal);
            if (highlighted) return Highlighted.Merge(Normal);

            return Normal;
        }

        public ImageAndColorSet WithNormalValue(ImageAndColor value) =>
            new ImageAndColorSet(value, Disabled, Highlighted, Active);

        public ImageAndColorSet WithDisabledValue(ImageAndColor value) =>
            new ImageAndColorSet(Normal, value, Highlighted, Active);

        public ImageAndColorSet WithHighlightedValue(ImageAndColor value) =>
            new ImageAndColorSet(Normal, Disabled, value, Active);

        public ImageAndColorSet WithActiveValue(ImageAndColor value) =>
            new ImageAndColorSet(Normal, Disabled, Highlighted, value);

        protected bool Equals(ImageAndColorSet other)
        {
            return Equals(_normal, other._normal) && Equals(_disabled, other._disabled) &&
                   Equals(_highlighted, other._highlighted) && Equals(_active, other._active);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((ImageAndColorSet) obj);
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
    public class ImageAndColorSetReactiveProperty : ReactiveProperty<ImageAndColorSet>
    {
        public ImageAndColorSetReactiveProperty()
        {
        }

        public ImageAndColorSetReactiveProperty(
            ImageAndColorSet initialValue) : base(initialValue)
        {
        }

        public ImageAndColorSetReactiveProperty(UniRx.IObservable<ImageAndColorSet> source) : base(source)
        {
        }

        public ImageAndColorSetReactiveProperty(UniRx.IObservable<ImageAndColorSet> source,
            ImageAndColorSet initialValue) : base(source, initialValue)
        {
        }
    }
}