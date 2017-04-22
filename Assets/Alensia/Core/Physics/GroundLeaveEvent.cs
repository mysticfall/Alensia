using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class GroundLeaveEvent : Signal<Collider, GroundLeaveEvent>
    {
    }
}