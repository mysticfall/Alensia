using System.Collections.Generic;
using Alensia.Core.UI.Event;

namespace Alensia.Core.UI
{
    public interface IComponent : IUIElement, IPointerPresenceAware
    {
        IComponent Parent { get; }

        IEnumerable<IComponent> Ancestors { get; }

        string Cursor { get; set; }

        UIStyle Style { get; set; }
    }
}