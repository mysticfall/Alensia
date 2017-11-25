using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Character.Customize;
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

        public ICustomizable Player => Alias?.Reference as ICustomizable;

        public IMorphSet Morphs => Player?.Morphs;

        public Transform ContentPanel;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            Alias.OnChange
                .Where(p => p != null)
                .Cast<IHumanoid, ICustomizable>()
                .Select(p => p.Morphs.OnMorphsChange)
                .Switch()
                .Subscribe(m => LoadMorphs(m.ToList()), Debug.LogError)
                .AddTo(this);
        }

        protected virtual void LoadMorphs(IReadOnlyList<IMorph> morphs)
        {
            ContentPanel.gameObject
                .GetComponentsInChildren<MorphControl>()
                .Select(c => c.GameObject)
                .ToList()
                .ForEach(Destroy);
        }
    }
}