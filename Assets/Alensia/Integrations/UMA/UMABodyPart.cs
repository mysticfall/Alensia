using System;
using System.Collections.Generic;
using Alensia.Core.Character.Morph;
using Malee;
using UMA;
using UnityEngine;

namespace Alensia.Integrations.UMA
{
    public class UMABodyPart : BodyPart, IUMARecipeItem
    {
        public IEnumerable<UMATextRecipe> Recipes => _recipes;

        [SerializeField, Reorderable] private UMATextRecipeList _recipes;
    }

    [Serializable]
    public class UMABodyPartList : ReorderableArray<UMABodyPart>
    {
    }
}