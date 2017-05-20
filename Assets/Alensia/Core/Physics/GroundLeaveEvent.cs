using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class GroundLeaveEvent : Signal<ISet<Collider>, GroundLeaveEvent>
    {
    }
}