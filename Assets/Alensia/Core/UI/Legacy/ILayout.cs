using UnityEngine;

namespace Alensia.Core.UI.Legacy
{
    public interface ILayout
    {
        Vector2 CalculateMinimumSize(IContainer container);

        Vector2 CalculatePreferredSize(IContainer container);

        void PerformLayout(IContainer container);
    }
}