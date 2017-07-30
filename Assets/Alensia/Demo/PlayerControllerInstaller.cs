using Alensia.Core.Character;
using Alensia.Core.Common;
using Alensia.Core.Control;
using Alensia.Core.Locomotion;
using Alensia.Core.Physics;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Demo
{
    public class PlayerControllerInstaller : MonoInstaller
    {
        public LeggedLocomotion.Settings Locomotion;

        public GroundDetectionSettings GroundDetection;

        protected virtual void OnValidate()
        {
            Assert.IsNotNull(Locomotion, "Locomotion != null");
            Assert.IsNotNull(GroundDetection, "GroundDetection != null");
        }

        public override void InstallBindings()
        {
            InstallModel();
            InstallAnimator();
            InstallPhysics();

            InstallLocomotion();
            InstallCharacter();
        }

        protected virtual void InstallModel()
        {
            Container.Bind<Transform>().FromInstance(GetComponent<Transform>()).AsSingle();
        }

        protected virtual void InstallPhysics()
        {
            Container.Bind<CapsuleCollider>().FromInstance(GetComponent<CapsuleCollider>()).AsSingle();
            Container.Bind<Rigidbody>().FromInstance(GetComponent<Rigidbody>()).AsSingle();

            Container.Bind<GroundDetectionSettings>().FromInstance(GroundDetection).AsSingle();
            Container.BindInterfacesAndSelfTo<CapsuleColliderGroundDetector>().AsSingle();
        }

        protected virtual void InstallAnimator()
        {
            Container.Bind<Animator>().FromInstance(GetComponent<Animator>()).AsSingle();
        }

        protected virtual void InstallLocomotion()
        {
            Container.Bind<LeggedLocomotion.Settings>().FromInstance(Locomotion);
            Container.BindInterfacesAndSelfTo<LeggedLocomotion>().AsSingle();
        }

        protected virtual void InstallCharacter()
        {
            Container.BindInterfacesAndSelfTo<HumanEyesight>().AsSingle();
            Container.BindInterfacesAndSelfTo<Humanoid>().AsSingle();

            Container
                .Bind<ReferenceInitializer<IHumanoid>>()
                .AsSingle()
                .WithArguments(PlayerController.PlayerAliasName)
                .NonLazy();
        }
    }
}