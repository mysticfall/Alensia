using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace Alensia.Core.Character
{
    public class Race
    {
        public string Name { get; }

        public IReadOnlyCollection<Sex> Sexes { get; }

        public Race(string name) : this(name, new List<Sex> {Sex.Male, Sex.Female})
        {
        }

        public Race(string name, IEnumerable<Sex> sexes)
        {
            Assert.IsNotNull(name, "name != null");

            var values = sexes?.ToList().AsReadOnly();

            Assert.IsNotNull(values, "sexes != null");
            Assert.IsTrue(values.Any(), "sexes.Any()");

            Name = name;
            Sexes = values;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Race) obj);
        }

        protected bool Equals(Race other) => string.Equals(Name, other.Name);

        public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;

        public static Race Human => new Race("Human");
    }
}