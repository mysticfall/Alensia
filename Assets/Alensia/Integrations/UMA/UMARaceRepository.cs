using System.Linq;
using Alensia.Core.Character;
using Malee;
using Optional;
using UMA;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Integrations.UMA
{
    public class UMARaceRepository : RaceRepository
    {
        [Inject]
        public UMAContext Context { get; }

        public RaceLibraryBase RaceLibrary => Context.raceLibrary;

        [SerializeField, Reorderable] private UMARaceMappingList _mappings;

        public virtual UMARecipeBase GetRacePreset(IRace race, Sex sex)
        {
            Assert.IsNotNull(race, "race != null");

            return _mappings?.ToList().Find(m => m.Name == race.Name)?.GetRacePreset(sex);
        }

        public virtual IRace GetRaceFromUMARace(string umaRace)
        {
            Assert.IsNotNull(umaRace, "umaRace != null");

            var raceName = _mappings?.ToList().Find(m => m.Matches(umaRace, Context))?.Name;

            return raceName != null ? this[raceName] : null;
        }

        public virtual Option<Sex> GetSexFromUMARace(string umaRace)
        {
            Assert.IsNotNull(umaRace, "umaRace != null");

            var mapping = _mappings?.ToList().Find(m => m.Matches(umaRace, Context));

            return mapping?.GetSex(umaRace, Context) ?? Option.None<Sex>();
        }
    }
}