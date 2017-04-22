using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class GroundHitEvent : Signal<Collider, GroundHitEvent>
    {
    }
}