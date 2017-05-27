using Alensia.Core.Geom;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI
{
    public interface IContentHolder : IBounded
    {
        string Text { get; set; }

        Texture Image { get; set; }

        string Tooltip { get; set; }

        GUIContent Content { get; }
    }

    public static class ContentHolderExtensions
    {
        public static Vector2 MinimumSize(this IContentHolder content, GUIStyle style)
        {
            Assert.IsNotNull(style, "style != null");

            float min, max;

            style.CalcMinMaxWidth(content.Content, out min, out max);

            var height = style.CalcHeight(content.Content, min);

            return new Vector2(min, height);
        }

        public static Vector2 PreferredSize(this IContentHolder content, GUIStyle style)
        {
            Assert.IsNotNull(style, "style != null");

            return style.CalcSize(content.Content);
        }
    }
}