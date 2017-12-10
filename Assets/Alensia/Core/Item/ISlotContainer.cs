using Alensia.Core.Collection;

namespace Alensia.Core.Item
{
    public interface ISlotContainer<out TSlot, TItem> : IDirectory<TItem>
        where TSlot : class, ISlot
        where TItem : class, ISlotItem<TSlot>
    {
        IDirectory<TSlot> Slots { get; }

        void Set(TItem item);

        void Clear(string key);
    }
}