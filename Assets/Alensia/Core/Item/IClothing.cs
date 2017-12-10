using Alensia.Core.Common;

namespace Alensia.Core.Item
{
    public interface IClothing : ISlotItem<ClothingSlot>, IFormInstance<IClothingForm>
    {
    }

    namespace Generic
    {
        public interface IClothing<out T> : IClothing, IFormInstance<T> where T : IClothingForm
        {
            new T Form { get; }
        }
    }
}