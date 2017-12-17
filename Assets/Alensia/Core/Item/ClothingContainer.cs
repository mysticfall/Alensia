using Alensia.Core.Character;
using Alensia.Core.Collection;
using Malee;

namespace Alensia.Core.Item
{
    public abstract class ClothingContainer<TItem, TList> : SlotContainer<ClothingSlot, IClothing, TItem, TList>,
        IClothingContainer
        where TItem : class, IClothing
        where TList : ReorderableArray<TItem>
    {
        protected abstract IRace Race { get; }

        public override IDirectory<ClothingSlot> Slots => Race.ClothingSlots;
    }
}