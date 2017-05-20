using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class GroundHitEvent : Signal<ISet<Collider>, GroundHitEvent>
    {
    }
}