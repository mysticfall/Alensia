using Alensia.Core.Game;
using Alensia.Core.I18n;
using Zenject;

namespace Alensia.Demo
{
    public class DemoInstaller : MonoInstaller
    {
        public LocaleService.Settings Locale;

        public ResourceSettings Translations;

        public override void InstallBindings()
        {
            InstallGame();
            InstallLocalization();
        }

        protected virtual void InstallGame()
        {
            Container.BindInterfacesAndSelfTo<Game>().AsSingle().NonLazy();
        }

        protected virtual void InstallLocalization()
        {
            Container.Bind<LocaleService.Settings>().FromInstance(Locale);
            Container.Bind<ResourceSettings>().FromInstance(Translations);

            Container.BindInterfacesAndSelfTo<LocaleService>().AsSingle();
            Container.BindInterfacesAndSelfTo<JsonResourceTranslator>().AsSingle();
        }
    }
}