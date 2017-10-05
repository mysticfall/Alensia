using UnityEngine;
using Zenject;

namespace Alensia.Core.Character
{
    public class CharacterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Transform>().FromInstance(GetComponent<Transform>()).AsSingle();
            Container.Bind<CapsuleCollider>().FromInstance(GetComponent<CapsuleCollider>()).AsSingle();
            Container.Bind<Rigidbody>().FromInstance(GetComponent<Rigidbody>()).AsSingle();
            Container.Bind<Animator>().FromInstance(GetComponent<Animator>()).AsSingle();
        }
    }
}