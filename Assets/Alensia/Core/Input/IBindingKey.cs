namespace Alensia.Core.Input
{
    public interface IBindingKey
    {
        string Id { get; }
    }
}

namespace Alensia.Core.Input.Generic
{
    // ReSharper disable once UnusedTypeParameter
    public interface IBindingKey<out T> : IBindingKey where T : IInput
    {
    }
}