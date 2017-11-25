using System;
using UnityEngine.Assertions;

namespace Alensia.Core.Character.Customize
{
    public class RangedMorph<T> : Morph<T> where T : IComparable<T>
    {
        public T MinValue { get; }

        public T MaxValue { get; }

        public RangedMorph(
            string name,
            T value,
            T defaultValue,
            T minValue,
            T maxValue) : base(name, value, defaultValue)
        {
            Assert.IsTrue(value.CompareTo(minValue) >= 0, "value >= minValue");
            Assert.IsTrue(value.CompareTo(maxValue) <= 0, "value <= maxValue");

            Assert.IsTrue(defaultValue.CompareTo(minValue) >= 0, "defaultValue >= minValue");
            Assert.IsTrue(defaultValue.CompareTo(maxValue) <= 0, "defaultValue <= maxValue");

            MinValue = minValue;
            MaxValue = maxValue;
        }

        protected override T Validate(T value)
        {
            if (value.CompareTo(MinValue) < 0 || value.CompareTo(MaxValue) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return base.Validate(value);
        }
    }
}