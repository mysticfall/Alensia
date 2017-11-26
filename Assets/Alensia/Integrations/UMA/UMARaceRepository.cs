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
    public class UMARaceRepository : ManagedDirectory<Race>, IRaceRepository
    {
        [Inject]
        public UMAContext Context { get; }

        public IEnumerable<Race> Races => _races;

        public RaceLibraryBase RaceLibrary => Context.raceLibrary;

        protected override IEnumerable<Race> Items => _races ?? Enumerable.Empty<Race>();

        [SerializeField] private List<UMARaceMapping> _mappings;

        private IReadOnlyList<Race> _races;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _races = _mappings.Select(m => m.GetRace()).ToList().AsReadOnly();
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
    }
}