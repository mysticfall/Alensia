using System.Collections.Generic;
using Alensia.Core.Collection;
using Alensia.Core.Common;
using Alensia.Core.Item;

namespace Alensia.Core.Character
{
    public interface IRace : IForm
    {
        IEnumerable<Sex> Sexes { get; }

        IDirectory<ClothingSlot> ClothingSlots { get; }
    }
}