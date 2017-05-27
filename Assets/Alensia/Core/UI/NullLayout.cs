using System.Linq;
using Alensia.Core.Geom;
using UnityEngine;

namespace Alensia.Core.UI
{
    public class NullLayout : Layout<object>
    {
        public override Vector2 CalculateMinimumSize(IContainer container)
        {
            return Components.Aggregate(Rect.zero, (area, child) => area.Add(child.Bounds)).size;
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