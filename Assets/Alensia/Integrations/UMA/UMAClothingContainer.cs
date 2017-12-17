using Alensia.Core.Character;
using Alensia.Core.Item;
using Zenject;

namespace Alensia.Integrations.UMA
{
    public class UMAClothingContainer : ClothingContainer<UMAClothing, UMAClothingList>, IUMAMorphSetContainer
    {
        [Inject]
        public UMAMorphSet MorphSet { get; }

        protected override IRace Race => MorphSet.Race;

        protected override void AddItem(UMAClothing item) => this.AddRecipeItem(item);

        protected override void RemoveItem(UMAClothing item) => this.RemoveRecipeItem(item);
    }
}