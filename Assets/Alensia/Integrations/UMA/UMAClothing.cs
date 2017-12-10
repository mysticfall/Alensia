using System;
using Alensia.Core.Item;
using Malee;

namespace Alensia.Integrations.UMA
{
    [Serializable]
    public class UMAClothing : Clothing<UMAClothingForm>
    {
    }

    [Serializable]
    public class UMAClothingList : ReorderableArray<UMAClothing>
    {
    }
}