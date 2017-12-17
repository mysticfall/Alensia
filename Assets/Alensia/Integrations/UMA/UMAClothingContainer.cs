using System;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Item;
using UMA;
using UMA.CharacterSystem;
using Zenject;

namespace Alensia.Integrations.UMA
{
    public class UMAClothingContainer : ClothingContainer<UMAClothing, UMAClothingList>
    {
        [Inject]
        public UMAMorphSet MorphSet { get; }

        protected override IRace Race => MorphSet.Race;

        protected override void ApplyClothing(UMAClothing item) =>
            ProcessSlot(item, (avatar, recipe) => avatar.SetSlot(recipe));
        
        protected override void RemoveClothing(UMAClothing item) =>
            ProcessSlot(item, (avatar, recipe) => avatar.ClearSlot(recipe.wardrobeSlot));

        protected void ProcessSlot(UMAClothing item, Action<DynamicCharacterAvatar, UMATextRecipe> process)
        {
            var umaRace = MorphSet.RaceData.raceName;
            var recipe = item.Form.Recipes.FirstOrDefault(r => r.compatibleRaces.Contains(umaRace));

            if (recipe == null) return;

            var avatar = MorphSet.Avatar;

            process(avatar, recipe);

            avatar.BuildCharacter();
        }
    }
}