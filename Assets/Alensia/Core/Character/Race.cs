using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Character
{
    [Serializable]
    public class Race
    {
        public string Name => _name;

        public IReadOnlyCollection<Sex> Sexes => _sexes;

        [SerializeField] private string _name;

        [SerializeField] private Sex[] _sexes;

        private Race()
        {
        }

        public Race(string name) : this(name, new List<Sex> {Sex.Male, Sex.Female})
        {
        }

        public Race(string name, IEnumerable<Sex> sexes)
        {
            Assert.IsNotNull(name, "name != null");

            var values = sexes?.ToList().AsReadOnly();

            Assert.IsNotNull(values, "sexes != null");

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.IsTrue(values.Any(), "sexes.Any()");

            _name = name;
            _sexes = values.ToArray();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Race) obj);
        }

        protected bool Equals(Race other) => string.Equals(Name, other.Name);

        public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;

        public static readonly Race Human = new Race("Human");
    }
}