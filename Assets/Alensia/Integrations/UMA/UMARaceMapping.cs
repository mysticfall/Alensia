using System;
using System.Collections.Generic;
using Alensia.Core.Character;
using Alensia.Core.Common;

namespace Alensia.Integrations.UMA
{
    [Serializable]
    public class UMARaceMapping : IEditorSettings
    {
        public string Name;

        public string Male;

        public string Female;

        public string Other;

        public Race GetRace()
        {
            var sexes = new List<Sex>(3);

            if (!string.IsNullOrEmpty(Male)) sexes.Add(Sex.Male);
            if (!string.IsNullOrEmpty(Female)) sexes.Add(Sex.Female);
            if (!string.IsNullOrEmpty(Other)) sexes.Add(Sex.Other);

            return new Race(Name, sexes);
        }

        public Sex? GetSex(string umaRace)
        {
            if (umaRace == Male) return Sex.Male;
            if (umaRace == Female) return Sex.Female;
            if (umaRace == Other) return Sex.Other;

            return null;
        }

        public string GetUMARace(Sex sex)
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

        public bool Matches(string umaRace) => umaRace == Male || umaRace == Female || umaRace == Other;
    }
}