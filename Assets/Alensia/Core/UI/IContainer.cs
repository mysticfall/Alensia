using System.Collections.Generic;

namespace Alensia.Core.UI
{
    public interface IContainer : IComponent
    {
        IList<IComponent> Children { get; }
    }
}