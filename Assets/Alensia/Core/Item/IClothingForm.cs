using Alensia.Core.Common;

namespace Alensia.Core.Item
{
    public interface IClothingForm : IForm
    {
        ClothingSlot Slot { get; }
    }
}