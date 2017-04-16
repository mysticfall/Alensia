using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Locomotion
{
    public interface IWalker : ILocomotion
    {
        WalkSpeedSettings MaximumSpeed { get; set; }

        Pacing Pacing { get; set; }

        event EventHandler<PacingChangeEventArgs> PacingChanged;

        void Walk(Vector2 direction, float heading);

        void Jump(Vector2 direction);
    }

    public class Pacing
    {
        public readonly string Name;

        public readonly float SpeedModifier;

        public Pacing(string name, float speedModifier)
        {
            Name = name;
            SpeedModifier = speedModifier;
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

    public class PacingChangeEventArgs : EventArgs
    {
        public readonly Pacing Pacing;

        public PacingChangeEventArgs(Pacing pacing)
        {
            Pacing = pacing;

            Assert.IsNotNull(Pacing);
        }
    }
}