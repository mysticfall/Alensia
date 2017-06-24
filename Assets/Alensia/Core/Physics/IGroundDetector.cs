using UniRx;
using UnityEngine;

namespace Alensia.Core.Physics
{
    public interface IGroundDetector
    {
        GroundDetectionSettings Settings { get; }

        Collider Target { get; }

        IReadOnlyReactiveCollection<Collider> Grounds { get; }

        IReadOnlyReactiveProperty<bool> Grounded { get; }

        IObservable<Unit> OnGroundHit { get; }

        IObservable<Unit> OnGroundLeave { get; }
    }
}