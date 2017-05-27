using UnityEngine;

namespace Alensia.Core.Geom
{
    public interface IBounded
    {
        Rect Bounds { get; set; }

        Vector2 Position { get; set; }

        Vector2 Size { get; set; }
    }
}