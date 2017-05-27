using System;
using System.Linq;
using Alensia.Core.Geom;
using ModestTree;
using UnityEngine;

namespace Alensia.Core.UI
{
    public class BoxLayout : Layout
    {
        public BoxOrientation Orientation { get; }

        public BoxLayout(BoxOrientation orientation)
        {
            Orientation = orientation;
        }

        public Vector2 CaculateSize(IContainer container, bool minimize = false)
            => container.Components.ToList()
                .Aggregate(Rect.zero, Fold(container, minimize)).size
                .GrowBy(container.Padding)
                .GrowBy(container.InnerPadding);

        public override Vector2 CalculateMinimumSize(IContainer container)
            => CaculateSize(container, true);

        public override Vector2 CalculatePreferredSize(IContainer container)
            => CaculateSize(container);

        public override void PerformLayout(IContainer container)
        {
            var layout = container.Layout as BoxLayout;

            Assert.IsNotNull(layout, "container.Layout != null");

            var isVertical = IsVertical(container);

            var offset = container is ICoordinatesHost ? Vector2.zero : container.Position;

            offset = offset
                .MoveBy(container.Padding)
                .MoveBy(container.InnerPadding);

            var size = container.Size
                .ShrinkBy(container.Padding)
                .ShrinkBy(container.InnerPadding);

            var children = container.Components.ToList();

            foreach (var child in children)
            {
                var bounds = new Rect
                {
                    x = offset.x,
                    y = offset.y,
                    width = isVertical ? size.x : child.PreferredSize.x,
                    height = isVertical ? child.PreferredSize.y : size.y
                };

                var margin = child.Margin;

                if (margin != null)
                {
                    bounds.x += margin.left;
                    bounds.y += margin.top;

                    if (isVertical)
                    {
                        bounds.width -= margin.left + margin.right;

                        offset.y += margin.top + margin.bottom;
                    }
                    else
                    {
                        bounds.height -= margin.top + margin.bottom;

                        offset.x += margin.left + margin.right;
                    }
                }

                child.Bounds = bounds;

                if (isVertical)
                {
                    offset.y += bounds.height;
                }
                else
                {
                    offset.x += bounds.width;
                }
            }
        }

        private static bool IsVertical(IContainer container)
        {
            var layout = container.Layout as BoxLayout;

            Assert.IsNotNull(layout, "container.Layout != null");

            return layout?.Orientation == BoxOrientation.Vertical;
        }

        private static Func<Rect, IComponent, Rect> Fold(
            IContainer container, bool minimize = false)
        {
            var isVertical = IsVertical(container);

            Func<Rect, IComponent, Rect> aggregator = (bounds, child) =>
            {
                var size = minimize ? child.MinimumSize : child.PreferredSize;

                size = size.GrowBy(child.Margin);

                if (isVertical)
                {
                    bounds.width = Mathf.Max(size.x, bounds.width);
                    bounds.height += size.y;
                }
                else
                {
                    bounds.width += size.x;
                    bounds.height = Mathf.Max(size.y, bounds.height);
                }

                return bounds;
            };

            return aggregator;
        }

        public enum BoxOrientation
        {
            Horizontal,
            Vertical
        }
    }
}