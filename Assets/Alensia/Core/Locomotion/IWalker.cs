using System;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Locomotion
{
    public interface IWalker
    {
        WalkSpeedSettings MaximumSpeed { get; set; }

        ILocomotion Locomotion { get; }

        Pacing Pacing { get; set; }

        event EventHandler<PacingChangeEventArgs> PacingChanged;

        void Walk(Vector2 direction);

        void WalkTo(Vector3 position);

        void Turn(float direction);

        void TurnTo(float heading);

        void Jump(Vector2 direction);
    }

    [Serializable]
    public class WalkSpeedSettings : IEditorSettings
    {
        [Range(0, 100)]
        public float Forward = 4;

        [Range(0, 100)]
        public float Backward = 1f;

        [Range(0, 100)]
        public float Sideway = 1f;

        [Range(0, 360)]
        public float Angular = 90;
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