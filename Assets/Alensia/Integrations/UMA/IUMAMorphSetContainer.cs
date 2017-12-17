using System;
using System.Linq;
using UMA;
using UMA.CharacterSystem;

namespace Alensia.Integrations.UMA
{
    public interface IUMAMorphSetContainer
    {
        UMAMorphSet MorphSet { get; }
    }

    public static class UMASlotContainerExtensions
    {
        public static void AddRecipeItem(this IUMAMorphSetContainer container, IUMARecipeItem item) =>
            ProcessSlot(container, item, (avatar, recipe) => avatar.SetSlot(recipe));

        public static void RemoveRecipeItem(this IUMAMorphSetContainer container, IUMARecipeItem item) =>
            ProcessSlot(container, item, (avatar, recipe) => avatar.ClearSlot(recipe.wardrobeSlot));

        private static void ProcessSlot(IUMAMorphSetContainer container, IUMARecipeItem item,
            Action<DynamicCharacterAvatar, UMATextRecipe> process)
        {
            var morphs = container.MorphSet;
            var umaRace = morphs.RaceData.raceName;
            var recipe = item.Recipes.FirstOrDefault(r => r.compatibleRaces.Contains(umaRace));

            if (recipe == null) return;

            var avatar = morphs.Avatar;

            process(avatar, recipe);

            avatar.BuildCharacter();
        }
    }
}