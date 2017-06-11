using UnityEngine;

namespace Alensia.Core.UI.Legacy
{
    public abstract class Layout : ILayout
    {
        public abstract Vector2 CalculateMinimumSize(IContainer container);

        public abstract Vector2 CalculatePreferredSize(IContainer container);

        public abstract void PerformLayout(IContainer container);
    }
}