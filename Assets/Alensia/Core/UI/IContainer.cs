using UnityEngine;

namespace Alensia.Core.UI
{
    public interface IContainer : IComponent, IComponentsHolder
    {
        ILayout Layout { get; }

        RectOffset InnerPadding { get; }

        void Pack();

        void InvalidateLayout();
    }
}