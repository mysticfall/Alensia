using UniRx;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Common
{
    public class ReferenceInitializer<T> : BaseObject where T : class
    {
        public IReferenceAlias<T> Alias { get; }

        public Lazy<T> Value { get; }

        public DiContainer Container { get; }

        public ReferenceInitializer(string name, Lazy<T> value, DiContainer container)
        {
            Assert.IsNotNull(container, "container != null");

            Value = value;
            Container = container;

            Alias = name == null
                ? container.Resolve<IReferenceAlias<T>>()
                : container.ResolveId<IReferenceAlias<T>>(name);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var value = Value.Value;

            var initializable = value as IBaseObject;

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