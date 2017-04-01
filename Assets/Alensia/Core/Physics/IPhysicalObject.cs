using UnityEngine;

namespace Alensia.Core.Physics
{
    public interface IPhysicalObject
    {
        Rigidbody Body { get; }
    }
}