using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Core.Control;
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

        public WalkSpeedSettings WalkSpeed;

        public GroundDetectionSettings GroundDetection;

        public AnimatedLocomotion.Settings AnimationSettings;

        public HeadMountedCamera.Settings FirstPersonCamera;

        public ThirdPersonCamera.Settings ThirdPersonCamera;

        public override void InstallBindings()
        {
            InstallModel();
            InstallAnimator();
            InstallPhysics();

            InstallInputControl();
            InstallLocomotion();
            InstallCameraControl();

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

            Container.Bind<Collider>().FromInstance(parent.GetComponent<Collider>());
            Container.Bind<Rigidbody>().FromInstance(parent.GetComponent<Rigidbody>());

            Container.DeclareSignal<GroundHitEvent>();
            Container.DeclareSignal<GroundLeaveEvent>();

            Container.Bind<GroundDetectionSettings>().FromInstance(GroundDetection);
            Container.BindInterfacesAndSelfTo<RayCastingGroundDetector>().AsSingle();
        }

        protected virtual void InstallAnimator()
        {
            var parent = transform.parent;

            Container.Bind<Animator>().FromInstance(parent.GetComponent<Animator>());
        }

        protected virtual void InstallInputControl()
        {
            Container.Bind<ViewSensitivity>().FromInstance(ViewSensitivity);

            Container.BindInterfacesAndSelfTo<DesktopInputManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirstAndThirdPersonController>().AsSingle().NonLazy();
        }

        protected virtual void InstallLocomotion()
        {
            Container.Bind<WalkSpeedSettings>().FromInstance(WalkSpeed);
            Container.Bind<AnimatedLocomotion.Settings>().FromInstance(AnimationSettings);

            Container.DeclareSignal<PacingChangeEvent>();
            Container.BindInterfacesAndSelfTo<WalkingLocomotion>().AsSingle();
        }

        protected virtual void InstallCameraControl()
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