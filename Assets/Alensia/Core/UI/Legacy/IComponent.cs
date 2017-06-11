using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.UI.Legacy
{
    public interface IComponent : ILayoutRegion, IPaintable, IStyleable, IActivatable, IHideable
    {
        IUIManager Manager { get; }

        Color? Color { get; set; }

        Color? BackgroundColor { get; set; }

        void InvalidateStyle();
    }
}