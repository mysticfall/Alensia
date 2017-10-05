using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Common
{
    public abstract class ReferenceInitializer<TVal, TRef> : ManagedMonoBehavior, INamed
        where TVal : class
        where TRef : class, IReferenceAlias<TVal>
    {
        public string Name => string.IsNullOrEmpty(_name) ? null : _name;

        public TRef Alias { get; private set; }

        [Inject]
        public Lazy<TVal> Value { get; }

        [Inject]
        public DiContainer Container { get; }

        [SerializeField] private string _name;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Alias = Name == null
                ? Container.Resolve<TRef>()
                : Container.ResolveId<TRef>(Name);

            var value = Value.Value;

            var initializable = value as IManagedObject;

            if (initializable == null || initializable.Initialized)
            {
                Alias.Reference = value;
            }
            else
            {
                initializable.OnInitialize
                    .Subscribe(_ => Alias.Reference = value)
                    .AddTo(this);
            }
        }
    }
}