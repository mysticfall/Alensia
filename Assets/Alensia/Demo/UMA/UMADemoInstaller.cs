using Alensia.Core.Camera;

namespace Alensia.Demo.UMA
{
    public class UMADemoInstaller : SceneInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.BindInterfacesAndSelfTo<UMADemoScene>().AsSingle().NonLazy();
        }

        protected override void InstallCameras()
        {
            base.InstallCameras();

            Container.BindInterfacesAndSelfTo<CharacterCamera>().AsSingle();
        }
    }
}