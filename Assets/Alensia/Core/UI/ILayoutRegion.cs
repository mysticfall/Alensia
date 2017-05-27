using Alensia.Core.Geom;
using UnityEngine;

namespace Alensia.Core.UI
{
    public interface ILayoutRegion : IBounded
    {
        IContainer Parent { get; set; }

        object LayoutConstraints { get; set; }

        Vector2 MinimumSize { get; }

        Vector2 PreferredSize { get; }

        RectOffset Padding { get; set; }

        RectOffset Margin { get; set; }
    }
}