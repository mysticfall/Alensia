using System;
using Alensia.Core.Character.Customize.Generic;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Character.Customize
{
    public class Morph<T> : IMorph<T>
    {
        public string Name { get; }

        public T Value
        {
            get { return _value.Value; }
            set { _value.Value = Validate(value); }
        }

        public T DefaultValue { get; }

        public IObservable<T> OnChange => _value;

        object IMorph.Value
        {
            get { return Value; }
            set { Value = (T) value; }
        }

        object IMorph.DefaultValue => DefaultValue;

        IObservable<object> IMorph.OnChange => OnChange.Cast<T, object>();

        private readonly IReactiveProperty<T> _value;

        public Morph(string name, T value, T defaultValue)
        {
            Assert.IsNotNull(name, "name != null");

            Name = name;
            DefaultValue = defaultValue;

            _value = new ReactiveProperty<T>(value);
        }

        /// <exception cref="System.ArgumentException">Thrown when given an invalid value.</exception>
        protected virtual T Validate(T value) => value;
    }
}