using Alensia.Core.Geom;
using UnityEngine;

namespace Alensia.Core.Physics
{
    public interface IPhysicalObject : ITransformable
    {
        Rigidbody Body { get; }
    }
}