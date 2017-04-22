using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class CollisionExitEvent : Signal<Collision, CollisionExitEvent>
    {
    }
}