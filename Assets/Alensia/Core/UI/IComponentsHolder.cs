using System.Collections.Generic;

namespace Alensia.Core.UI
{
    public interface IComponentsHolder
    {
        IReadOnlyList<IComponent> Components { get; }

        bool Contains(IComponent child);

        void Add(IComponent child);

        void Remove(IComponent child);

        void RemoveAll();
    }
}