using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class GroundHitEvent : Signal<IEnumerable<Collider>, GroundHitEvent>
    {
    }
}