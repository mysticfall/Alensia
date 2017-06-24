using Alensia.Core.Physics;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Locomotion
{
    public interface IWalkingLocomotion : ILocomotion
    {
        WalkSpeedSettings MaximumSpeed { get; }

        IGroundDetector GroundDetector { get; }

        IReactiveProperty<Pacing> Pacing { get; }

        void Walk(Vector2 direction, float heading);

        void Jump(Vector2 direction);
    }
}