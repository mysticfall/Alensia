using System.Collections.Generic;

namespace Alensia.Core.UI
{
    public interface IContainer : IComponent
    {
        IEnumerable<IComponent> Children { get; }
    }
}