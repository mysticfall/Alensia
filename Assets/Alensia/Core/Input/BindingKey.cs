using Alensia.Core.Input.Generic;
using UnityEngine.Assertions;

namespace Alensia.Core.Input
{
    public class BindingKey<T> : IBindingKey<T> where T : IInput
    {
        public string Id { get; }

        public BindingKey(string id)
        {
            Assert.IsNotNull(id, "id != null");

            Id = id;
        }

        public override bool Equals(object obj) => Id.Equals((obj as BindingKey<T>)?.Id);

        public override int GetHashCode() => Id.GetHashCode();
    }
}