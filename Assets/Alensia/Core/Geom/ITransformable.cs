using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Geom
{
    public interface ITransformable: IGameObject
    {
        Transform Transform { get; }
    }
}