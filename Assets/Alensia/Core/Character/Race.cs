using System;
using System.Collections.Generic;
using Alensia.Core.I18n;
using UnityEngine;

namespace Alensia.Core.Character
{
    [Serializable]
    public class Race : ScriptableObject, IRace
    {
        public string Name => _name;

        public TranslatableText DisplayName => _displayName;

        public IEnumerable<Sex> Sexes => _sexes;

        [SerializeField] private string _name;

        [SerializeField] private TranslatableText _displayName;

        [SerializeField] private Sex[] _sexes;
    }
}