using Alensia.Core.Physics;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Locomotion
{
    public interface ILeggedLocomotion : ILocomotion
    {
        WalkSpeedSettings MaximumSpeed { get; }

        IGroundDetector GroundDetector { get; }

        Pacing Pacing { get; set; }

        IObservable<Pacing> OnPacingChange { get; }

        void Walk(Vector2 direction, float heading);

        void Jump(Vector2 direction);
    }
}