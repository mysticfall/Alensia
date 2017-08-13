using Alensia.Core.Character;
using Alensia.Core.Character.Morph;
using Alensia.Core.Common;
using Alensia.Core.Control;
using Alensia.Integrations.UMA;
using UMA.CharacterSystem;

namespace Alensia.Demo.UMA
{
    public class UMACharacterInstaller : CharacterInstaller
    {
        protected override void InstallCharacter()
        {
            var avatar = GetComponent<DynamicCharacterAvatar>();

            Container.Bind<DynamicCharacterAvatar>().FromInstance(avatar).AsSingle();

            Container.BindInterfacesAndSelfTo<HumanEyesight>().AsSingle();
            Container.BindInterfacesAndSelfTo<UMAMorphSet>().AsSingle();
            Container.BindInterfacesAndSelfTo<MorphableHumanoid>().AsSingle();

            Container
                .BindInterfacesAndSelfTo<ReferenceInitializer<IHumanoid>>()
                .AsSingle()
                .WithArguments(PlayerController.PlayerAliasName)
                .NonLazy();
        }
    }
}