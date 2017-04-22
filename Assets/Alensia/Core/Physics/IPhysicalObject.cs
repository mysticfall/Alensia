using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Physics
{
    public interface IPhysicalObject: ITransformable
    {
        Rigidbody Body { get; }
    }
}