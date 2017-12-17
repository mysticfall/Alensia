using Alensia.Core.Collection;

namespace Alensia.Core.Character.Morph
{
    public interface IMorphableRace : IRace
    {
        IDirectory<BodyPartSlot> BodyPartSlots { get; }
    }
}