using Alensia.Core.Camera;
using Alensia.Core.Character;
using Alensia.Core.Common;
using Alensia.Core.Control;
using Alensia.Core.Game;
using Alensia.Core.I18n;
using Alensia.Core.Input;
using Alensia.Core.UI;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Demo
{
    public class GameInstaller : MonoInstaller
    {
        public CameraControl.ViewSensitivity ViewSensitivity;

        public HeadMountedCamera.Settings FirstPersonCamera;

        public ThirdPersonCamera.Settings ThirdPersonCamera;

        public LocaleService.Settings Locale;

        public ResourceSettings Translations;

        public UIManager.Settings UI;

        protected virtual void OnValidate()
        {
            Assert.IsNotNull(ViewSensitivity, "ViewSensitivity != null");
            Assert.IsNotNull(FirstPersonCamera, "FirstPersonCamera != null");
            Assert.IsNotNull(ThirdPersonCamera, "ThirdPersonCamera != null");
        }

        public override void InstallBindings()
        {
            InstallGame();
            InstallLocalization();
            InstallUI();
            InstallControls();
            InstallCameras();
            InstallReferences();
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

        protected virtual void InstallUI()
        {
            Container.Bind<UIManager.Settings>().FromInstance(UI);

            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();
        }

        protected virtual void InstallCameras()
        {
            Container.Bind<Camera>().FromInstance(Camera.main);

            Container.Bind<HeadMountedCamera.Settings>().FromInstance(FirstPersonCamera);
            Container.Bind<ThirdPersonCamera.Settings>().FromInstance(ThirdPersonCamera);

            Container.BindInterfacesAndSelfTo<HeadMountedCamera>().AsSingle();
            Container.BindInterfacesAndSelfTo<ThirdPersonCamera>().AsSingle();

            Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle();
        }

        protected virtual void InstallControls()
        {
            Container.Bind<CameraControl.ViewSensitivity>().FromInstance(ViewSensitivity);

            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameControl>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerCameraControl>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovementControl>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        }

        protected virtual void InstallReferences()
        {
            Container
                .Bind<IReferenceAlias<IHumanoid>>()
                .WithId(PlayerController.PlayerAliasName)
                .To<ReferenceAlias<IHumanoid>>()
                .AsSingle();
        }
    }
}