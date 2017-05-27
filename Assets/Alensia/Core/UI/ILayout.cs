using UnityEngine;

namespace Alensia.Core.UI
{
    public interface ILayout
    {
        Vector2 CalculateMinimumSize(IContainer container);

        Vector2 CalculatePreferredSize(IContainer container);

        void PerformLayout(IContainer container);
    }
}