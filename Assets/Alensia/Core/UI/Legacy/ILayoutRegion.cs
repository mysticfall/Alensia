using Alensia.Core.Geom;
using UnityEngine;

namespace Alensia.Core.UI.Legacy
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

    public static class LayoutRegionExtensions
    {
        public static void Pack(this ILayoutRegion region)
        {
            region.Size = region.PreferredSize;
        }

        public static void CenterOnScreen(this ILayoutRegion region)
        {
            var size = region.Size;

            var x = (Screen.width - size.x) / 2;
            var y = (Screen.height - size.y) / 2;

            region.Position = new Vector2(x, y);
        }
    }
}