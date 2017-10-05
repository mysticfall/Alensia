using System;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Locomotion
{
    [Serializable]
    public class Pacing
    {
        public string Name => _name;

        public float SpeedModifier => _speedModifier;

        [SerializeField] private string _name;

        [SerializeField] private float _speedModifier;

        private Pacing()
        {
        }

        public Pacing(string name, float speedModifier)
        {
            _name = name;
            _speedModifier = speedModifier;
        }

        public static Pacing Walking(float speedModifier = 1)
        {
            return new Pacing("Walking", speedModifier);
        }

        public static Pacing Crawling(float speedModifier = 0.2f)
        {
            return new Pacing("Crawling", speedModifier);
        }

        public static Pacing Crouching(float speedModifier = 0.5f)
        {
            return new Pacing("Crouching", speedModifier);
        }

        public static Pacing Running(float speedModifier = 2)
        {
            return new Pacing("Running", speedModifier);
        }
    }

    [Serializable]
    public class PacingReactiveProperty : ReactiveProperty<Pacing>
    {
        public PacingReactiveProperty()
        {
        }

        public PacingReactiveProperty(
            Pacing initialValue) : base(initialValue)
        {
        }

        public PacingReactiveProperty(IObservable<Pacing> source) : base(source)
        {
        }

        public PacingReactiveProperty(IObservable<Pacing> source, Pacing initialValue) : base(source, initialValue)
        {
        }
    }
}