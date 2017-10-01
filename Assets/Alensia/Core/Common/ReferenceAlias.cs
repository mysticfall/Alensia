using System;
using UniRx;
using Zenject;

namespace Alensia.Core.Common
{
    public class ReferenceAlias<T> : IReferenceAlias<T> where T : class
    {
        public T Reference
        {
            get { return _reference.Value; }
            set { _reference.Value = value; }
        }

        public IObservable<T> OnChange => _reference;

        private readonly IReactiveProperty<T> _reference;

        [Inject]
        public ReferenceAlias() : this(null)
        {
        }

        public ReferenceAlias(T reference)
        {
            _reference = new ReactiveProperty<T>(reference);
        }
    }
}