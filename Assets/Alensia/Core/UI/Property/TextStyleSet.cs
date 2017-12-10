using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI.Property
{
    [Serializable]
    public class TextStyleSet : ITransitionalProperty<TextStyle>, IMergeableProperty<TextStyleSet>
    {
        public TextStyle Normal => _normal;

        public TextStyle Disabled => _disabled;

        public TextStyle Highlighted => _highlighted;

        public TextStyle Active => _active;

        [SerializeField] private TextStyle _normal;

        [SerializeField] private TextStyle _disabled;

        [SerializeField] private TextStyle _highlighted;

        [SerializeField] private TextStyle _active;

        public TextStyleSet() : this(null, null, null, null)
        {
        }

        public TextStyleSet(
            TextStyle normal,
            TextStyle disabled,
            TextStyle highlighted,
            TextStyle active)
        {
            _normal = normal ?? new TextStyle();
            _disabled = disabled ?? new TextStyle();
            _highlighted = highlighted ?? new TextStyle();
            _active = active ?? new TextStyle();
        }

        public TextStyleSet(TextStyleSet source)
        {
            Assert.IsNotNull(source);

            _normal = source.Normal;
            _disabled = source.Disabled;
            _highlighted = source.Highlighted;
            _active = source.Active;
        }

        public TextStyleSet Merge(TextStyleSet other)
        {
            return other == null
                ? this
                : new TextStyleSet(
                    Normal.Merge(other.Normal),
                    Disabled.Merge(other.Disabled),
                    Highlighted.Merge(other.Highlighted),
                    Active.Merge(other.Active));
        }

        public TextStyle ValueFor(IInteractableComponent component) =>
            ValueFor(!component.Interactable, component.Highlighted, component.Interacting);

        public TextStyle ValueFor(bool disabled, bool highlighted, bool active)
        {
            if (disabled) return Disabled.Merge(Normal);
            if (active) return Active.Merge(Highlighted).Merge(Normal);
            if (highlighted) return Highlighted.Merge(Normal);

            return Normal;
        }

        public TextStyleSet WithNormalValue(TextStyle value) =>
            new TextStyleSet(value, Disabled, Highlighted, Active);

        public TextStyleSet WithDisabledValue(TextStyle value) =>
            new TextStyleSet(Normal, value, Highlighted, Active);

        public TextStyleSet WithHighlightedValue(TextStyle value) =>
            new TextStyleSet(Normal, Disabled, value, Active);

        public TextStyleSet WithActiveValue(TextStyle value) =>
            new TextStyleSet(Normal, Disabled, Highlighted, value);

        protected bool Equals(TextStyleSet other)
        {
            return Equals(_normal, other._normal) && Equals(_disabled, other._disabled) &&
                   Equals(_highlighted, other._highlighted) && Equals(_active, other._active);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((TextStyleSet) obj);
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
    public class TextStyleSetReactiveProperty : ReactiveProperty<TextStyleSet>
    {
        public TextStyleSetReactiveProperty()
        {
        }

        public TextStyleSetReactiveProperty(
            TextStyleSet initialValue) : base(initialValue)
        {
        }

        public TextStyleSetReactiveProperty(IObservable<TextStyleSet> source) : base(source)
        {
        }

        public TextStyleSetReactiveProperty(IObservable<TextStyleSet> source,
            TextStyleSet initialValue) : base(source, initialValue)
        {
        }
    }
}