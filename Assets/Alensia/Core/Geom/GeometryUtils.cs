using UnityEngine;

namespace Alensia.Core.Geom
{
    public static class GeometryUtils
    {
        public static float NormalizeAspectAngle(float degrees)
        {
            var value = degrees;

            while (value < 0) value += 360;

            return value > 180 ? value - 360 : value;
        }

        public static Rect Add(this Rect source, Rect other)
        {
            var x1 = Mathf.Min(source.x, other.x);
            var y1 = Mathf.Min(source.y, other.y);

            var x2 = Mathf.Max(source.x + source.width, other.x + other.width);
            var y2 = Mathf.Max(source.y + source.height, other.y + other.height);

            return new Rect
            {
                x = x1,
                y = y1,
                width = x2 - x1,
                height = y2 - y1
            };
        }

        public static Vector2 MoveBy(this Vector2 source, RectOffset padding)
        {
            if (padding == null) return source;

            source.x += padding.left;
            source.y += padding.top;

            return source;
        }

        public static Vector2 GrowBy(this Vector2 source, RectOffset padding)
        {
            if (padding == null) return source;

            source.x += padding.left + padding.right;
            source.y += padding.top + padding.bottom;

            return source;
        }

        public static Vector2 ShrinkBy(this Vector2 source, RectOffset padding)
        {
            if (padding == null) return source;

            source.x -= padding.left + padding.right;
            source.y -= padding.top + padding.bottom;

            return source;
        }
    }
}