using System;
using System.Collections.Generic;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Character
{
    [Serializable]
    public class Race : Form, IRace
    {
        public IEnumerable<Sex> Sexes => _sexes;

        [SerializeField] private Sex[] _sexes;
    }
}