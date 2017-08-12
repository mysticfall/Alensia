using UMA;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Integrations.UMA
{
    public class UMASystemInstaller : MonoInstaller
    {
        public UMAContext Context;

        public UMARaceRepository.Settings Races;

        protected void OnValidate()
        {
            Assert.IsNotNull(Context, "Context != null");
            Assert.IsNotNull(Races, "Races != null");
        }

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<UMAContext>()
                .FromInstance(Context)
                .AsSingle();

            Container.Bind<RaceLibraryBase>().FromInstance(Context.raceLibrary).AsSingle();
            Container.Bind<SlotLibraryBase>().FromInstance(Context.slotLibrary).AsSingle();
            Container.Bind<OverlayLibraryBase>().FromInstance(Context.overlayLibrary).AsSingle();

            Container.BindInterfacesAndSelfTo<UMARaceRepository.Settings>().FromInstance(Races);
            Container.BindInterfacesAndSelfTo<UMARaceRepository>().AsSingle();
        }
    }
}