namespace Alensia.Core.UI.Property
{
    public interface IMergeableProperty<T> where T : IMergeableProperty<T>
    {
        T Merge(T other);
    }
}