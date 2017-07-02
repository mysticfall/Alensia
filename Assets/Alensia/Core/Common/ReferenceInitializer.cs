using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Common
{
    public class ReferenceInitializer<T> where T : class
    {
        public ReferenceInitializer(string name, T value, DiContainer container)
        {
            Assert.IsNotNull(container, "container != null");

            var alias = name == null
                ? container.Resolve<IReferenceAlias<T>>()
                : container.ResolveId<IReferenceAlias<T>>(name);

            alias.Reference = value;
        }
    }
}