using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Core.Control;
using Alensia.Core.Input;
using Alensia.Core.Locomotion;
using Alensia.Core.Physics;
using UnityEngine;
using Zenject;

namespace Alensia.Demo.Controller
{
    public class PlayerControllerInstaller : MonoInstaller<PlayerControllerInstaller>
    {
        public Camera Camera;

        public ViewSensitivity ViewSensitivity;

        public WalkingLocomotion.Settings Locomotion;

        public GroundDetectionSettings GroundDetection;

        public HeadMountedCamera.Settings FirstPersonCamera;

        public ThirdPersonCamera.Settings ThirdPersonCamera;

        public override void InstallBindings()
        {
            InstallModel();
            InstallAnimator();
            InstallPhysics();

            InstallControls();
            InstallLocomotion();
            InstallCameras();

            InstallCharacter();
        }

        protected virtual void InstallModel()
        {
            var parent = transform.parent;

            Container.Bind<Transform>().FromInstance(parent);
        }

        protected virtual void InstallPhysics()
        {
            var parent = transform.parent;

            Container.Bind<CapsuleCollider>().FromInstance(parent.GetComponent<CapsuleCollider>());
            Container.Bind<Rigidbody>().FromInstance(parent.GetComponent<Rigidbody>());

            Container.DeclareSignal<GroundHitEvent>();
            Container.DeclareSignal<GroundLeaveEvent>();

            Container.Bind<GroundDetectionSettings>().FromInstance(GroundDetection);
            Container.BindInterfacesAndSelfTo<CapsuleColliderGroundDetector>().AsSingle();
        }

        protected virtual void InstallAnimator()
        {
            var parent = transform.parent;

            Container.Bind<Animator>().FromInstance(parent.GetComponent<Animator>());
        }

        protected virtual void InstallControls()
        {
            Container.Bind<ViewSensitivity>().FromInstance(ViewSensitivity);

            Container.DeclareSignal<BindingChangeEvent>();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerCameraControl<IHumanoid>>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovementControl>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle().NonLazy();
        }

        protected virtual void InstallLocomotion()
        {
            Container.Bind<WalkingLocomotion.Settings>().FromInstance(Locomotion);

            Container.DeclareSignal<PacingChangeEvent>();
            Container.BindInterfacesAndSelfTo<WalkingLocomotion>().AsSingle();
        }

        protected virtual void InstallCameras()
        {
            Container.Bind<Camera>().FromInstance(Camera);

            Container.Bind<HeadMountedCamera.Settings>().FromInstance(FirstPersonCamera);
            Container.Bind<ThirdPersonCamera.Settings>().FromInstance(ThirdPersonCamera);

            Container.BindInterfacesAndSelfTo<HeadMountedCamera>().AsSingle();
            Container.BindInterfacesAndSelfTo<ThirdPersonCamera>().AsSingle();

            Container.DeclareSignal<CameraChangeEvent>();
            Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();
        }

        protected virtual void InstallCharacter()
        {
            Container.Bind<IHumanoid>().To<Humanoid>().AsSingle();
        }
    }
}