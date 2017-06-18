namespace Alensia.Core.Common
{
    public interface IDirectory<out T>
    {
        bool Contains(string key);

        T this[string key] { get; }
    }
}