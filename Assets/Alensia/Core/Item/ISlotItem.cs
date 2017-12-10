using Alensia.Core.Common;

namespace Alensia.Core.Item
{
    public interface ISlotItem<out T> : ILabelled where T : ISlot
    {
        T Slot { get; }
    }
}