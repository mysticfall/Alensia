using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Common;
using Optional;
using UMA;
using UnityEngine.Assertions;

namespace Alensia.Integrations.UMA
{
    public class UMARaceRepository : IRaceRepository
    {
        public UMAContext Context { get; }

        public IReadOnlyList<Race> Races { get; }

        public RaceLibraryBase RaceLibrary => Context.raceLibrary;

        protected IReadOnlyList<UMARaceMapping> Mappings => _mappings;

        private readonly List<UMARaceMapping> _mappings;

        private readonly IDictionary<string, Race> _raceMappings;

        public UMARaceRepository(Settings settings, UMAContext context)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(settings.RaceMappings, "RaceMappings != null");
            Assert.IsNotNull(context, "context != null");

            _mappings = settings.RaceMappings.ToList();
            _raceMappings = _mappings.ToDictionary(k => k.Name, e => e.GetRace());

            Races = _mappings.Select(m => m.GetRace()).ToList().AsReadOnly();
            Context = context;
        }

        public virtual UMARecipeBase GetRacePreset(Race race, Sex sex)
        {
            Assert.IsNotNull(race, "race != null");

            return _mappings.Find(m => m.Name == race.Name)?.GetRacePreset(sex);
        }

        public virtual Race GetRaceFromUMARace(string umaRace)
        {
            Assert.IsNotNull(umaRace, "umaRace != null");

            return _mappings.Find(m => m.Matches(umaRace, Context))?.GetRace();
        }

        public virtual Option<Sex> GetSexFromUMARace(string umaRace)
        {
            Assert.IsNotNull(umaRace, "umaRace != null");

            var mapping = _mappings.Find(m => m.Matches(umaRace, Context));

            return mapping?.GetSex(umaRace, Context) ?? Option.None<Sex>();
        }

        public bool Contains(string key) => _raceMappings.ContainsKey(key);

        public Race this[string key] => _raceMappings[key];

        [Serializable]
        public class Settings : IEditorSettings
        {
            public UMARaceMapping[] RaceMappings;
        }
    }
}