using Alensia.Core.Input.Generic;
using UnityEngine.Assertions;

namespace Alensia.Core.Input
{
    public class BindingKey<T> : IBindingKey<T> where T : IInput
    {
        public string Id { get; private set; }

        public BindingKey(string id)
        {
            Assert.IsNotNull(id, "id != null");

            Id = id;
        }

        public override bool Equals(object obj)
        {
            var item = obj as BindingKey<T>;

            return item != null && Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}