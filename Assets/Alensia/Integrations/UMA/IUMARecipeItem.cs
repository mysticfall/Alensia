using System;
using System.Collections.Generic;
using Alensia.Core.Common;
using Malee;
using UMA;

namespace Alensia.Integrations.UMA
{
    public interface IUMARecipeItem
    {
        IEnumerable<UMATextRecipe> Recipes { get; }
    }

    [Serializable]
    public class UMATextRecipeList : ReorderableArray<UMATextRecipe>
    {
    }
}