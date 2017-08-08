using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Common;
using UMA;
using UnityEngine.Assertions;

namespace Alensia.Integrations.UMA
{
    public class UMARaceRepository : IRaceRepository
    {
        public IReadOnlyList<Race> Races { get; }

        public RaceLibraryBase RaceLibrary { get; }

        protected IReadOnlyList<UMARaceMapping> Mappings => _mappings;

        private readonly List<UMARaceMapping> _mappings;

        private readonly IDictionary<string, Race> _raceMappings;

        public UMARaceRepository(Settings settings, RaceLibraryBase raceLibrary)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(settings.RaceMappings, "RaceMappings != null");

            RaceLibrary = raceLibrary;

            _mappings = settings.RaceMappings.ToList();
            _raceMappings = _mappings.ToDictionary(k => k.Name, e => e.GetRace());

            Races = _mappings.Select(m => m.GetRace()).ToList().AsReadOnly();
        }

        public virtual string GetUMARace(Race race, Sex sex)
        {
            Assert.IsNotNull(race, "race != null");

            return _mappings.Find(m => m.Name == race.Name)?.GetUMARace(sex);
        }

        public virtual Race GetRaceFromUMARace(string umaRace)
        {
            Assert.IsNotNull(umaRace, "umaRace != null");

            return _mappings.Find(m => m.Matches(umaRace))?.GetRace();
        }

        public virtual Sex GetSexFromUMARace(string umaRace)
        {
            Assert.IsNotNull(umaRace, "umaRace != null");

            var sex = _mappings.Find(m => m.Matches(umaRace))?.GetSex(umaRace);

            if (!sex.HasValue)
            {
                throw new ArgumentOutOfRangeException(nameof(umaRace));
            }

            return sex.Value;
        }

        public bool Contains(string key) => _raceMappings.ContainsKey(key);

        public Race this[string key] => _raceMappings[key];

        [Serializable]
        public class Settings : IEditorSettings
        {
            public UMARaceMapping[] RaceMappings =
            {
                new UMARaceMapping
                {
                    Name = Race.Human.Name,
                    Male = "HumanMaleHighPoly",
                    Female = "HumanFemaleHighPoly"
                }
            };
        }
    }
}