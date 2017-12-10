using System;
using System.Collections.Generic;
using Alensia.Core.Item;
using Malee;
using UMA;
using UnityEngine;

namespace Alensia.Integrations.UMA
{
    public class UMAClothingForm : ClothingForm
    {
        public IEnumerable<UMATextRecipe> Recipes => _recipes;

        [SerializeField, Reorderable] private UMATextRecipeList _recipes;
    }

    [Serializable]
    internal class UMATextRecipeList : ReorderableArray<UMATextRecipe>
    {
    }
}