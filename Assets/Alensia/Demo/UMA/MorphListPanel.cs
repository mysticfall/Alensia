using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Character.Morph;
using Alensia.Core.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Demo.UMA
{
    public abstract class MorphListPanel : CharacterPanel
    {
        [Inject]
        public IRaceRepository RaceRepository { get; }

        public IMorphable Player => Alias?.Reference as IMorphable;

        public IMorphSet Morphs => Player?.Morphs;

        public Transform ContentPanel;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            Alias.OnChange
                .Where(p => p != null)
                .Cast<IHumanoid, IMorphable>()
                .Select(p => p.Morphs.OnMorphsChange)
                .Switch()
                .Subscribe(m => LoadMorphs(m.ToList()), Debug.LogError)
                .AddTo(this);
        }

        protected virtual void LoadMorphs(IReadOnlyList<IMorph> morphs)
        {
            ContentPanel.gameObject
                .GetComponentsInChildren<ControlPanel>()
                .Select(c => c.GameObject)
                .ToList()
                .ForEach(Destroy);
        }
    }
}