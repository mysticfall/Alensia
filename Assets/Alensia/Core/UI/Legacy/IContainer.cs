using UnityEngine;

namespace Alensia.Core.UI.Legacy
{
    public interface IContainer : IComponent, IComponentsHolder
    {
        ILayout Layout { get; }

        RectOffset InnerPadding { get; }

        void InvalidateLayout();
    }
}