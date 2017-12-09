using System;
using Malee;

namespace Alensia.Core.Character
{
    public enum Sex
    {
        Male = 1,
        Female = 2,
        Other = 0
    }

    [Serializable]
    internal class SexList : ReorderableArray<Sex>
    {
    }
}