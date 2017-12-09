using System;
using System.Collections.Generic;
using Alensia.Core.Common;
using Malee;
using UnityEngine;

namespace Alensia.Core.Character
{
    [Serializable]
    public class Race : Form, IRace
    {
        public IEnumerable<Sex> Sexes => _sexes;

        [SerializeField, Reorderable] private SexList _sexes;
    }

    [Serializable]
    internal class RaceList : ReorderableArray<Race>
    {
    }
}