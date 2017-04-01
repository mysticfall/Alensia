using System;
using Alensia.Core.Animation;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Locomotion
{
    public interface IWalker : ILocomotion, IAnimatable
    {
        WalkSpeedSettings MaximumSpeed { get; set; }

        WalkAnimationVariables WalkAnimationVariables { get; set; }

        Pacing Pacing { get; set; }

        event EventHandler<PacingChangeEventArgs> PacingChanged;

        void Walk(Vector2 direction);

        void Turn(float degrees);

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

    [Serializable]
    public class WalkAnimationVariables : IEditorSettings
    {
        public string Moving = "Moving";

        public string ForwardVelocity = "Forward Velocity";

        public string SidewayVelocity = "Sideway Velocity";
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