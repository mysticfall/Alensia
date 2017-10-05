using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Common
{
    public abstract class ReferenceAlias<T> : ManagedMonoBehavior, IReferenceAlias<T> where T : class
    {
        public string Name => _name;
       
        public T Reference
        {
            get { return _property.Value; }
            set { _property.Value = value; }
        }

        public IObservable<T> OnChange => _property;

        [SerializeField] private string _name;

        [InjectOptional]
        private T _reference;

        private readonly IReactiveProperty<T> _property;

        protected ReferenceAlias()
        {
            _property = new ReactiveProperty<T>(_reference);
        }
    }
}