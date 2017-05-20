using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Core.Control;
using Alensia.Core.Input;
using Alensia.Core.Locomotion;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Demo.Controller
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public GameObject Player;

        public ViewSensitivity ViewSensitivity;

        public HeadMountedCamera.Settings FirstPersonCamera;

        public ThirdPersonCamera.Settings ThirdPersonCamera;

        protected void OnValidate()
        {
            Assert.IsNotNull(Player, "Player != null");
            Assert.IsNotNull(ViewSensitivity, "ViewSensitivity != null");
            Assert.IsNotNull(FirstPersonCamera, "FirstPersonCamera != null");
            Assert.IsNotNull(ThirdPersonCamera, "ThirdPersonCamera != null");
        }

        public override void InstallBindings()
        {
            InstallControls();
            InstallCameras();
            InstallPlayer();
        }

        protected void InstallCameras()
        {
            Container.Bind<Camera>().FromInstance(Camera.main);

            Container.Bind<HeadMountedCamera.Settings>().FromInstance(FirstPersonCamera);
            Container.Bind<ThirdPersonCamera.Settings>().FromInstance(ThirdPersonCamera);

            Container.BindInterfacesAndSelfTo<HeadMountedCamera>().AsSingle();
            Container.BindInterfacesAndSelfTo<ThirdPersonCamera>().AsSingle();

            Container.DeclareSignal<CameraChangeEvent>();
            Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();
        }

        protected void InstallControls()
        {
            Container.Bind<ViewSensitivity>().FromInstance(ViewSensitivity);

            Container.DeclareSignal<BindingChangeEvent>();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameControl>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerCameraControl<IHumanoid>>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovementControl>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle().NonLazy();
        }

        protected void InstallPlayer()
        {
            Container
                .Bind<IHumanoid>()
                .FromSubContainerResolve()
                .ByNewPrefab(Player)
                .AsSingle();

            Container
                .Bind<IWalkingLocomotion>()
                .FromResolveGetter<IHumanoid>(h => h.Locomotion)
                .AsSingle();
        }
    }
}