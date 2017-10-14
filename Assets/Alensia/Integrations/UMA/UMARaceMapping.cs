using System;
using System.Collections.Generic;
using Alensia.Core.Character;
using Alensia.Core.Common;
using Optional;
using UMA;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Integrations.UMA
{
    [Serializable]
    public class UMARaceMapping : IEditorSettings
    {
        public string Name => _name;

        public UMARecipeBase Male => _male;

        public UMARecipeBase Female => _female;

        public UMARecipeBase Other => _other;

        [SerializeField] private string _name;

        [SerializeField] private UMARecipeBase _male;

        [SerializeField] private UMARecipeBase _female;

        [SerializeField] private UMARecipeBase _other;

        private readonly IDictionary<Sex, RaceData> _raceData;

        public UMARaceMapping()
        {
            _raceData = new Dictionary<Sex, RaceData>();
        }

        public Race GetRace()
        {
            var sexes = new List<Sex>(3);

            if (Male != null) sexes.Add(Sex.Male);
            if (Female != null) sexes.Add(Sex.Female);
            if (Other != null) sexes.Add(Sex.Other);

            return new Race(Name, sexes);
        }

        public Option<Sex> GetSex(string umaRace, UMAContext context)
        {
            Assert.IsNotNull(umaRace, "umaRace != null");

            if (umaRace == GetRaceData(Sex.Male, context)?.raceName) return Sex.Male.Some();
            if (umaRace == GetRaceData(Sex.Female, context)?.raceName) return Sex.Female.Some();
            if (umaRace == GetRaceData(Sex.Other, context)?.raceName) return Sex.Other.Some();

            return Option.None<Sex>();
        }

        public UMARecipeBase GetRacePreset(Sex sex)
        {
            switch (sex)
            {
                case Sex.Male:
                    return Male;
                case Sex.Female:
                    return Female;
                default:
                    return Other;
            }
        }

        public RaceData GetRaceData(Sex sex, UMAContext context)
        {
            RaceData data = null;

            lock (_raceData)
            {
                if (_raceData.ContainsKey(sex))
                {
                    return _raceData[sex];
                }

                UMARecipeBase recipe;

                switch (sex)
                {
                    case Sex.Male:
                        recipe = Male;
                        break;
                    case Sex.Female:
                        recipe = Female;
                        break;
                    default:
                        recipe = Other;
                        break;
                }

                if (recipe != null)
                {
                    var umaData = new UMAData.UMARecipe();

                    recipe.Load(umaData, context);

                    data = umaData.raceData;

                    _raceData[sex] = data;
                }
            }

            return data;
        }

        public bool Matches(string umaRace, UMAContext context) =>
            umaRace != null && (
                umaRace == GetRaceData(Sex.Male, context)?.raceName ||
                umaRace == GetRaceData(Sex.Female, context)?.raceName ||
                umaRace == GetRaceData(Sex.Other, context)?.raceName);
    }
}