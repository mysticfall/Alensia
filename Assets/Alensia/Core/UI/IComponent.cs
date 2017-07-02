using System.Collections.Generic;

namespace Alensia.Core.UI
{
    public interface IComponent : IUIElement, IPointerPresenceAware
    {
        IComponent Parent { get; }

        IEnumerable<IComponent> Ancestors { get; }

        string Cursor { get; set; }
    }
}