using Alensia.Core.Character;
using Alensia.Core.Common;
using Alensia.Core.Control;
using Alensia.Core.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Demo.UMA
{
    public abstract class CharacterPanel : ComponentHandler<Panel>
    {
        [Inject(Id = PlayerController.PlayerAliasName)]
        public IReferenceAlias<IHumanoid> Alias { get; }

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            Alias.OnChange
                .Where(p => p != null)
                .Subscribe(LoadCharacter, Debug.LogError)
                .AddTo(this);
        }

        protected virtual void LoadCharacter(IHumanoid character)
        {
        }
    }
}