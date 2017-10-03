using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Character.Morph;
using Alensia.Core.UI;
using UniRx;
using UnityEngine;

namespace Alensia.Demo.UMA
{
    public class RangedMorphPanel : MorphListPanel
    {
        public Dropdown SexMenu;

        public GameObject MorphSliderPrefab;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            SexMenu.OnValueChange
                .Where(_ => Morphs != null)
                .Select(v => (Sex) Enum.Parse(typeof(Sex), v))
                .Subscribe(OnSexChange, Debug.LogError)
                .AddTo(this);
        }

        protected override void LoadCharacter(IHumanoid character)
        {
            SexMenu.Value = character.Sex.ToString();
        }

        protected override void LoadMorphs(IReadOnlyList<IMorph> morphs)
        {
            base.LoadMorphs(morphs);

            var ranges = morphs
                .OrderBy(m => m.Name)
                .Select(m => m as RangedMorph<float>)
                .Where(m => m != null);

            foreach (var morph in ranges)
            {
                var slider = Context.Instantiate<MorphSlider>(
                    MorphSliderPrefab,
                    ContentPanel);

                slider.Morph = morph;
            }
        }

        protected virtual void OnSexChange(Sex sex) => Morphs.Sex = sex;
    }
}