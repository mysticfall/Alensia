using Alensia.Core.Character.Generic;
using Alensia.Core.Common;
using Alensia.Core.Locomotion;
using Alensia.Core.Sensor;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Character
{
    public abstract class Character<TVision, TLocomotion> : ManagedMonoBehavior, ICharacter<TVision, TLocomotion>
        where TVision : class, IVision
        where TLocomotion : class, ILocomotion
    {
        public string Name => _name;

        public abstract Race Race { get; }

        public abstract Sex Sex { get; }

        public abstract Transform Head { get; }

        public abstract TVision Vision { get; }

        public abstract TLocomotion Locomotion { get; }

        [Inject]
        public Animator Animator { get; }

        [Inject]
        public override Transform Transform { get; }

        IVision ISeeing.Vision => Vision;

        ILocomotion ILocomotive.Locomotion => Locomotion;

        [SerializeField] private string _name;
    }
}