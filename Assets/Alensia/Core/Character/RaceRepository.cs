using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace Alensia.Core.Character
{
    public class RaceRepository : IRaceRepository
    {
        public IReadOnlyList<Race> Races { get; }

        private readonly IDictionary<string, Race> _mappings;

        public RaceRepository() : this(new List<Race> {Race.Human})
        {
        }

        public RaceRepository(ICollection<Race> races)
        {
            Assert.IsNotNull(races, "races != null");

            Races = races.ToList().AsReadOnly();

            _mappings = new Dictionary<string, Race>();

            foreach (var race in Races)
            {
                _mappings[race.Name] = race;
            }
        }

        public bool Contains(string key) => _mappings.ContainsKey(key);

        public Race this[string key] => _mappings[key];
    }
}