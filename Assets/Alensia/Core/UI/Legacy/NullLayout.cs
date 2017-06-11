using System.Linq;
using Alensia.Core.Geom;
using UnityEngine;

namespace Alensia.Core.UI.Legacy
{
    public class NullLayout : Layout
    {
        public override Vector2 CalculateMinimumSize(IContainer container)
        {
            var children = container.Components.ToList();

            return children.Aggregate(Rect.zero, (area, child) => area.Add(child.Bounds)).size;
        }

        public override Vector2 CalculatePreferredSize(IContainer container) =>
            CalculateMinimumSize(container);

        public override void PerformLayout(IContainer container)
        {
            var position = container.Position;
            var size = CalculateMinimumSize(container);

            var bounds = container.Bounds.Add(new Rect(position, size));

            container.Bounds = bounds;
        }
    }
}