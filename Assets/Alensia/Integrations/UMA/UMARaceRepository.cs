using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Common;
using Optional;
using UMA;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Integrations.UMA
{
    public class UMARaceRepository : ManagedMonoBehavior, IRaceRepository
    {
        [Inject]
        public UMAContext Context { get; }

        public IEnumerable<Race> Races => _races;

        public RaceLibraryBase RaceLibrary => Context.raceLibrary;

        [SerializeField] private List<UMARaceMapping> _mappings;

        private IReadOnlyList<Race> _races;

        private IDictionary<string, Race> _raceMap;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _races = _mappings.Select(m => m.GetRace()).ToList().AsReadOnly();
            _raceMap = _mappings.ToDictionary(k => k.Name, e => e.GetRace());
        }

        public virtual UMARecipeBase GetRacePreset(Race race, Sex sex)
        {
            Assert.IsNotNull(race, "race != null");

            return _mappings?.Find(m => m.Name == race.Name)?.GetRacePreset(sex);
        }

        public virtual Race GetRaceFromUMARace(string umaRace)
        {
            Assert.IsNotNull(umaRace, "umaRace != null");

            return _mappings?.Find(m => m.Matches(umaRace, Context))?.GetRace();
        }

        public virtual Option<Sex> GetSexFromUMARace(string umaRace)
        {
            Assert.IsNotNull(umaRace, "umaRace != null");

            var mapping = _mappings?.Find(m => m.Matches(umaRace, Context));

            return mapping?.GetSex(umaRace, Context) ?? Option.None<Sex>();
        }

        public bool Contains(string key) => _raceMap != null && _raceMap.ContainsKey(key);

        public Race this[string key] => _raceMap?[key];
    }
}