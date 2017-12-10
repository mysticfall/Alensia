namespace Alensia.Core.Collection
{
    public interface IDirectory<out T>
    {
        bool Contains(string key);

        T this[string key] { get; }
    }
}