using UnityEngine;

namespace Alensia.Core.Physics
{
    public interface ICollisionDetector
    {
        Collider Target { get; }

        CollisionEnterEvent CollisionEntered { get; }

        CollisionExitEvent CollisionExited { get; }
    }
}