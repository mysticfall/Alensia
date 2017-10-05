using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Physics
{
    public interface IGroundDetector
    {
        Collider Target { get; }

        LayerMask GroundLayer { get; set; }

        float Tolerance { get; set; }

        ISet<Collider> GroundContacts { get; }

        bool Grounded { get; }

        IObservable<Unit> OnGroundHit { get; }

        IObservable<Unit> OnGroundLeave { get; }

        IObservable<bool> OnGroundedStateChange { get; }

        IObservable<ISet<Collider>> OnGroundContactsChange { get; }
    }
}