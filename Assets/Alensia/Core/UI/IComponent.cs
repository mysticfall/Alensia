using System.Collections.Generic;

namespace Alensia.Core.UI
{
    public interface IComponent : IUIElement
    {
        IComponent Parent { get; }

        IEnumerable<IComponent> Ancestors { get; }

        UIStyle Style { get; set; }
    }
}