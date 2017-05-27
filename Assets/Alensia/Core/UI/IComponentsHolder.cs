using System.Collections.Generic;
using UniRx;

namespace Alensia.Core.UI
{
    public interface IComponentsHolder
    {
        IReadOnlyList<IComponent> Components { get; }

        bool Contains(IComponent child);

        void Add(IComponent child);

        void Remove(IComponent child);

        void RemoveAll();

        IObservable<IComponent> ComponentAdded { get; }

        IObservable<IComponent> ComponentRemoved { get; }
    }
}