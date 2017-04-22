using UnityEngine;

namespace Alensia.Core.Physics
{
    public interface IGroundDetector
    {
        GroundDetectionSettings Settings { get; }

        Collider Target { get; }

        Collider Ground { get; }

        bool Grounded { get; }

        GroundHitEvent GroundHit { get; }

        GroundLeaveEvent GroundLeft { get; }
    }
}