using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class CollisionEnterEvent : Signal<Collision, CollisionEnterEvent>
    {
    }
}