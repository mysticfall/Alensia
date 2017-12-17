using System;
using System.Collections.Generic;
using Alensia.Core.Item;
using Malee;
using UMA;

namespace Alensia.Integrations.UMA
{
    [Serializable]
    public class UMAClothing : Clothing<UMAClothingForm>, IUMARecipeItem
    {
        public IEnumerable<UMATextRecipe> Recipes => Form.Recipes;
    }

    [Serializable]
    public class UMAClothingList : ReorderableArray<UMAClothing>
    {
    }
}