using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Character.Morph;
using Alensia.Core.Common;
using Alensia.Core.Control;
using Alensia.Core.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Demo.UMA
{
    public class CharacterCustomizer : UIHandler<Panel>
    {
        [Inject(Id = PlayerController.PlayerAliasName), NonSerialized] public IReferenceAlias<IHumanoid> Alias;

        [Inject, NonSerialized] public IPlayerController Controller;

        [Inject, NonSerialized] public IRaceRepository RaceRepository;

        public IMorphable Player => Alias?.Reference as IMorphable;

        public IMorphSet Morphs => Player?.Morphs;

        public Dropdown SexMenu;

        public Transform MorphListPanel;

        public GameObject MorphSliderPrefab;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            Controller.DisablePlayerControl();

            Alias.OnChange
                .Where(p => p != null)
                .Subscribe(LoadCharacter)
                .AddTo(this);

            Alias.OnChange
                .Where(p => p != null)
                .Cast<IHumanoid, IMorphable>()
                .Select(p => p.Morphs.OnMorphsChange)
                .Switch()
                .Subscribe(LoadMorphs)
                .AddTo(this);

            SexMenu.OnValueChange
                .Where(_ => Morphs != null)
                .Select(v => (Sex) Enum.Parse(typeof(Sex), v))
                .Subscribe(OnSexChange)
                .AddTo(this);
        }

        protected virtual void LoadCharacter(IHumanoid character)
        {
            SexMenu.Value = character.Sex.ToString();
        }

        protected virtual void LoadMorphs(IEnumerable<IMorph> morphs)
        {
            var children = MorphListPanel.gameObject.GetComponentsInChildren<MorphSlider>();

            children.ToList().ForEach(c => Destroy(c.GameObject));

            var list = morphs.OrderBy(m => m.Name);

            foreach (var morph in list)
            {
                var slider = Context.Instantiate<MorphSlider>(
                    MorphSliderPrefab,
                    MorphListPanel);

                slider.Morph = (RangedMorph<float>) morph;
            }
        }

        protected virtual void OnSexChange(Sex sex)
        {
            Morphs.Sex = sex;
        }
    }
}