using System;

namespace Alensia.Core.Interaction
{
    public interface IHighlightable
    {
        bool Highlighted { get; }

        IObservable<bool> OnHighlightedStateChange { get; }
    }
}