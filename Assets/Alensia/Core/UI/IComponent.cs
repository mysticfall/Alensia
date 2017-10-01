using System.Collections.Generic;
using Alensia.Core.UI.Screen;

namespace Alensia.Core.UI
{
    public interface IComponent : IUIElement, IStylable
    {
        IScreen Screen { get; }

        IComponent Parent { get; }

        IEnumerable<IComponent> Ancestors { get; }

        IEnumerable<IComponentHandler> Handlers { get; }
    }
}