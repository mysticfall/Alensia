using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Item;
using Zenject;

namespace Alensia.Integrations.UMA
{
    public class UMAClothingContainer : ClothingContainer<UMAClothing, UMAClothingList>
    {
        [Inject]
        public UMAMorphSet MorphSet { get; }

        protected override IRace Race => MorphSet.Race;

        protected override void ApplyClothing(UMAClothing item)
        {
            var umaRace = MorphSet.RaceData.raceName;
            var recipe = item.Form.Recipes.FirstOrDefault(r => r.compatibleRaces.Contains(umaRace));

            if (recipe != null)
            {
                var avatar = MorphSet.Avatar;

                avatar.SetSlot(recipe);
                avatar.BuildCharacter();
            }
        }

        protected override void RemoveClothing(string key, UMAClothing item)
        {
            throw new System.NotImplementedException();
        }
    }
}