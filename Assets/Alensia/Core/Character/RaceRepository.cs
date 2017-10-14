using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Character
{
    public class RaceRepository : ManagedMonoBehavior, IRaceRepository
    {
        public IEnumerable<Race> Races => _races;

        private IDictionary<string, Race> _mappings;

        [SerializeField] private Race[] _races;

        public RaceRepository() : this(new List<Race> {Race.Human})
        {
        }

        public RaceRepository(ICollection<Race> races)
        {
            Assert.IsNotNull(races, "races != null");

            _races = races.ToArray();
        }

        protected override void OnInitialized()
        {
            _races = _races ?? new Race[0];
            _mappings = new Dictionary<string, Race>();

            foreach (var race in Races)
            {
                _mappings[race.Name] = race;
            }

            base.OnInitialized();
        }

        public bool Contains(string key) => _mappings.ContainsKey(key);

        public Race this[string key] => _mappings[key];
    }
}