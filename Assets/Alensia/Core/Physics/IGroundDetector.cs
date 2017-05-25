using System.Collections.Generic;
using UnityEngine;

namespace Alensia.Core.Physics
{
    public interface IGroundDetector
    {
        GroundDetectionSettings Settings { get; }

        Collider Target { get; }

        ISet<Collider> Grounds { get; }

        bool Grounded { get; }

        GroundHitEvent GroundHit { get; }

        GroundLeaveEvent GroundLeft { get; }
    }
}