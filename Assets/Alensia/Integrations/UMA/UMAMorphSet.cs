using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Character.Morph;
using Alensia.Core.Character.Morph.Generic;
using Alensia.Core.Common;
using UMA;
using UMA.CharacterSystem;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Integrations.UMA
{
    public class UMAMorphSet : MorphSet
    {
        public DynamicCharacterAvatar Avatar { get; }

        public UMAData Data => Avatar?.umaData;

        public RaceData RaceData => Avatar?.activeRace.data;

        public UMARaceRepository RaceRepository { get; }

        private readonly IDictionary<string, DNAKey> _dnaMappings;

        public UMAMorphSet(DynamicCharacterAvatar avatar, UMARaceRepository raceRepository)
        {
            Assert.IsNotNull(avatar, "avatar != null");
            Assert.IsNotNull(raceRepository, "raceRepository != null");

            Avatar = avatar;
            RaceRepository = raceRepository;

            _dnaMappings = new Dictionary<string, DNAKey>();

            var umaRace = avatar.activeRace.name;

            Race = RaceRepository.GetRaceFromUMARace(umaRace);
            Sex = RaceRepository.GetSexFromUMARace(umaRace).ValueOr(Sex.Other);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Avatar.RecipeUpdated.AsObservable()
                .Subscribe(_ => UpdateMorphs())
                .AddTo(this);

            UpdateMorphs();
        }

        protected override IEnumerable<IMorph> CreateMorphs()
        {
            _dnaMappings.Clear();

            var allDna = Data.GetAllDna();

            foreach (var dna in allDna)
            {
                var hash = dna.DNATypeHash;

                for (var i = 0; i < dna.Names.Length; i++)
                {
                    _dnaMappings[dna.Names[i]] = new DNAKey(hash, i);
                }
            }

            var morphs = allDna.SelectMany(CreateMorphs);
            var colors = Avatar.characterColors.Colors.Select(CreateMorph);

            return morphs.Concat(colors);
        }

        protected virtual IEnumerable<IMorph> CreateMorphs(UMADnaBase dna)
        {
            var values = dna.Names.Zip(dna.Values, (n, v) => new Tuple<string, float>(n, v));

            return values.Select(tuple =>
            {
                var name = tuple.Item1;
                var value = tuple.Item2;

                return new RangedMorph<float>(name, value, 0, 0, 1);
            });
        }

        protected virtual IMorph CreateMorph(DynamicCharacterAvatar.ColorValue colorData)
        {
            return new Morph<Color>(colorData.name, colorData.color, Color.black);
        }

        protected override void ApplyMorph(IMorph morph)
        {
            var dna = morph as IMorph<float>;

            if (dna != null && _dnaMappings.ContainsKey(morph.Name))
            {
                var mapping = _dnaMappings[morph.Name];

                ApplyDNA(mapping.Hash, mapping.Index, dna.Value);
            }

            var color = morph as IMorph<Color>;

            if (color != null)
            {
                ApplyColor(color.Name, color.Value);
            }
        }

        protected virtual void ApplyDNA(int dnaIndex, int valueIndex, float value)
        {
            Data.GetDna(dnaIndex).SetValue(valueIndex, value);
            Avatar.ForceUpdate(true);
        }

        protected virtual void ApplyColor(string name, Color color)
        {
            var data = Avatar.GetColor(name);

            data.color = color;

            Avatar.SetColor(name, data);
        }

        protected override void ChangeSex(Sex sex) => ChangeUmaRace(Race, sex);

        protected override void ChangeRace(Race race) => ChangeUmaRace(race, Sex);

        protected virtual void ChangeUmaRace(Race race, Sex sex)
        {
            var preset = RaceRepository.GetRacePreset(race, sex);

            if (preset == null)
            {
                throw new ArgumentException(
                    $"Cannot determine UMA race: sex = '{sex}', race = '{race}'.");
            }

            Avatar.LoadFromRecipe(preset);
        }

        private struct DNAKey
        {
            public readonly int Hash;

            public readonly int Index;

            public DNAKey(int hash, int index)
            {
                Hash = hash;
                Index = index;
            }
        }
    }
}