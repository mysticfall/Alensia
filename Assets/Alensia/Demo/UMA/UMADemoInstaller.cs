using Alensia.Core.Camera;

namespace Alensia.Demo.UMA
{
    public class UMADemoInstaller : GameInstaller
    {
        protected override void InstallGame()
        {
            Container.BindInterfacesAndSelfTo<UMADemo>().AsSingle().NonLazy();
        }

        protected override void InstallCameras()
        {
            base.InstallCameras();

            Container.BindInterfacesAndSelfTo<CharacterCamera>().AsSingle();
        }
    }
}