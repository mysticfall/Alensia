using Alensia.Core.Collection;
using Alensia.Core.Item;
using Malee;

namespace Alensia.Core.Character.Morph
{
    public abstract class BodyPartContainer<TItem, TList> : SlotContainer<BodyPartSlot, IBodyPart, TItem, TList>,
        IBodyPartContainer
        where TItem : class, IBodyPart
        where TList : ReorderableArray<TItem>
    {
        protected abstract IMorphableRace Race { get; }

        public override IDirectory<BodyPartSlot> Slots => Race.BodyPartSlots;
    }
}